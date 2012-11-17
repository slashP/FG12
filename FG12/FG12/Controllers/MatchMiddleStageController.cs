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
    public class MatchMiddleStageController : Controller
    {
        private DataContext db = new DataContext();

        //
        // GET: /MatchMiddleStage/

        public ActionResult Index()
        {
            var matchesmiddlestage = db.MatchesMiddleStage.Include(m => m.HomeTeam).Include(m => m.AwayTeam);
            return View(matchesmiddlestage.ToList());
        }

        //
        // GET: /MatchMiddleStage/Details/5

        public ActionResult Details(int id = 0)
        {
            MatchMiddleStage matchmiddlestage = db.MatchesMiddleStage.Find(id);
            if (matchmiddlestage == null)
            {
                return HttpNotFound();
            }
            return View(matchmiddlestage);
        }

        //
        // GET: /MatchMiddleStage/Create

        public ActionResult Create()
        {
            ViewBag.HomeTeamId = new SelectList(db.Teams, "Id", "Name");
            ViewBag.AwayTeamId = new SelectList(db.Teams, "Id", "Name");
            return View();
        }

        //
        // POST: /MatchMiddleStage/Create

        [HttpPost]
        public ActionResult Create(MatchMiddleStage matchmiddlestage)
        {
            var matchError = GetMatchError(matchmiddlestage);
            if (ModelState.IsValid && string.IsNullOrEmpty(matchError))
            {
                db.MatchesMiddleStage.Add(matchmiddlestage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HomeTeamId = new SelectList(db.Teams, "Id", "Name", matchmiddlestage.HomeTeamId);
            ViewBag.AwayTeamId = new SelectList(db.Teams, "Id", "Name", matchmiddlestage.AwayTeamId);
            ViewBag.Error = matchError;
            return View(matchmiddlestage);
        }

        private string GetMatchError(MatchMiddleStage matchmiddlestage)
        {
            var homeTeam = db.Teams.Find(matchmiddlestage.HomeTeamId);
            var awayTeam = db.Teams.Find(matchmiddlestage.AwayTeamId);
            var isInSameGroup = homeTeam.GroupMiddleStageId == awayTeam.GroupMiddleStageId;
            var homeTeamOpponents = homeTeam.HomeMatchesMiddleStage.Select(x => x.AwayTeamId).Concat(homeTeam.AwayMatchesMiddleStage.Select(x => x.HomeTeamId)).Distinct();
            var hasNotPlayedBefore = !homeTeamOpponents.Contains(awayTeam.Id);
            var error = isInSameGroup ? string.Empty : "Ikke i samme gruppe";
            error += hasNotPlayedBefore ? string.Empty : "Kamp allerede registrert";
            error += matchmiddlestage.HomeTeamId != matchmiddlestage.AwayTeamId ? string.Empty : "Samme lag";
            return error;
        }

        //
        // GET: /MatchMiddleStage/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MatchMiddleStage matchmiddlestage = db.MatchesMiddleStage.Find(id);
            if (matchmiddlestage == null)
            {
                return HttpNotFound();
            }
            ViewBag.HomeTeamId = new SelectList(db.Teams, "Id", "Name", matchmiddlestage.HomeTeamId);
            ViewBag.AwayTeamId = new SelectList(db.Teams, "Id", "Name", matchmiddlestage.AwayTeamId);
            return View(matchmiddlestage);
        }

        //
        // POST: /MatchMiddleStage/Edit/5

        [HttpPost]
        public ActionResult Edit(MatchMiddleStage matchmiddlestage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(matchmiddlestage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HomeTeamId = new SelectList(db.Teams, "Id", "Name", matchmiddlestage.HomeTeamId);
            ViewBag.AwayTeamId = new SelectList(db.Teams, "Id", "Name", matchmiddlestage.AwayTeamId);
            return View(matchmiddlestage);
        }

        //
        // GET: /MatchMiddleStage/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MatchMiddleStage matchmiddlestage = db.MatchesMiddleStage.Find(id);
            if (matchmiddlestage == null)
            {
                return HttpNotFound();
            }
            return View(matchmiddlestage);
        }

        //
        // POST: /MatchMiddleStage/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            MatchMiddleStage matchmiddlestage = db.MatchesMiddleStage.Find(id);
            db.MatchesMiddleStage.Remove(matchmiddlestage);
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