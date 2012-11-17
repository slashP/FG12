using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FG12.Models;

namespace FG12.Controllers
{
    [Authorize]
    public class MatchController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /Match/

        public ActionResult Index()
        {
            var matches = db.Matches.Include(m => m.HomeTeam).Include(m => m.AwayTeam);
            return View(matches.ToList());
        }

        //
        // GET: /Match/Details/5

        public ActionResult Details(int id = 0)
        {
            Match match = db.Matches.Find(id);
            if (match == null)
            {
                return HttpNotFound();
            }
            return View(match);
        }

        //
        // GET: /Match/Create

        public ActionResult Create()
        {
            ViewBag.HomeTeamId = new SelectList(db.Teams, "Id", "Name");
            ViewBag.AwayTeamId = new SelectList(db.Teams, "Id", "Name");
            return View();
        }

        //
        // POST: /Match/Create

        [HttpPost]
        public ActionResult Create(Match match)
        {
            var matchError = GetMatchError(match);
            if (ModelState.IsValid && string.IsNullOrEmpty(matchError)) {
                db.Matches.Add(match);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HomeTeamId = new SelectList(db.Teams, "Id", "Name", match.HomeTeamId);
            ViewBag.AwayTeamId = new SelectList(db.Teams, "Id", "Name", match.AwayTeamId);
            
            ViewBag.Error = matchError;
            return View(match);
        }

        private string GetMatchError(Match match)
        {
            var homeTeam = db.Teams.Find(match.HomeTeamId);
            var awayTeam = db.Teams.Find(match.AwayTeamId);
            var isInSameGroup = homeTeam.GroupId == awayTeam.GroupId;
            var homeTeamOpponents =
                homeTeam.HomeMatches.Select(x => x.AwayTeamId).Concat(homeTeam.AwayMatches.Select(x => x.HomeTeamId)).Distinct();
            var hasNotPlayedBefore = !homeTeamOpponents.Contains(awayTeam.Id);
            var error = isInSameGroup ? string.Empty : "Ikke i samme gruppe";
            error += hasNotPlayedBefore ? string.Empty : "Kamp allerede registrert";
            error += match.HomeTeamId != match.AwayTeamId ? string.Empty : "Samme lag";
            return error;
        }

        //
        // GET: /Match/Edit/5
        [AllowAnonymous]
        public ActionResult Edit(int id = 0)
        {
            Match match = db.Matches.Find(id);
            if (match == null)
            {
                return HttpNotFound();
            }
            ViewBag.HomeTeamId = new SelectList(db.Teams, "Id", "Name", match.HomeTeamId);
            ViewBag.AwayTeamId = new SelectList(db.Teams, "Id", "Name", match.AwayTeamId);
            return View(match);
        }

        //
        // POST: /Match/Edit/5

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Edit(Match match)
        {
            if (ModelState.IsValid)
            {
                db.Entry(match).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.HomeTeamId = new SelectList(db.Teams, "Id", "Name", match.HomeTeamId);
            ViewBag.AwayTeamId = new SelectList(db.Teams, "Id", "Name", match.AwayTeamId);
            return View(match);
        }

        //
        // GET: /Match/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Match match = db.Matches.Find(id);
            if (match == null)
            {
                return HttpNotFound();
            }
            return View(match);
        }

        //
        // POST: /Match/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Match match = db.Matches.Find(id);
            db.Matches.Remove(match);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}