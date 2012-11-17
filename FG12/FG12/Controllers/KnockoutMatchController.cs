using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EuroApi.Models;
using FG12.Models;

namespace FG12.Controllers
{
    [Authorize]
    public class KnockoutMatchController : Controller
    {
        private readonly DataContext _db = new DataContext();

        public ActionResult GenerateKnockoutMatchesQuarterFinals()
        {
            var teams = _db.Teams.Include(t => t.HomeMatchesMiddleStage).Include(t => t.AwayMatchesMiddleStage).Include(t => t.GroupMiddleStage).ToList();
            var teamsByGroup = teams.Select(x => x.GroupMiddleStage.Name).Distinct().Select(id => teams.Where(x => x.GroupMiddleStage.Name == id)).ToList();
            var orderedTeams = new List<List<Team>>();
            teamsByGroup.ForEach(t => orderedTeams.Add(Standing.SortTeamsMiddleStage(t.ToList())));
            if(orderedTeams.Count != 4 || orderedTeams.Sum(x => x.Count) != 16) {
                return null;
            }
            _db.KnockoutMatches.Add(new KnockoutMatch
                {
                    HomeTeamId = orderedTeams[0][0].Id,
                    AwayTeamId = orderedTeams[1][1].Id,
                    Type = KnockoutMatch.QUARTERFINAL
                });
            _db.KnockoutMatches.Add(new KnockoutMatch
                {
                    HomeTeamId = orderedTeams[1][0].Id,
                    AwayTeamId = orderedTeams[0][1].Id,
                    Type = KnockoutMatch.QUARTERFINAL
                });
            _db.KnockoutMatches.Add(new KnockoutMatch
                {
                    HomeTeamId = orderedTeams[2][0].Id,
                    AwayTeamId = orderedTeams[3][1].Id,
                    Type = KnockoutMatch.QUARTERFINAL
                });
            _db.KnockoutMatches.Add(new KnockoutMatch
                {
                    HomeTeamId = orderedTeams[3][0].Id,
                    AwayTeamId = orderedTeams[2][1].Id,
                    Type = KnockoutMatch.QUARTERFINAL
                });
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GenerateKnockoutMatchesSemiFinals()
        {
            var quarterFinals = _db.KnockoutMatches.Include(k => k.HomeTeam).Include(k => k.AwayTeam).Where(x => x.Type == KnockoutMatch.QUARTERFINAL).ToList();
            if(quarterFinals.Count != 4) {
                return null;
            }
            _db.KnockoutMatches.Add(new KnockoutMatch
                {
                    HomeTeamId = quarterFinals[0].Winner().Id,
                    AwayTeamId = quarterFinals[1].Winner().Id,
                    Type = KnockoutMatch.SEMIFINAL
                });
            _db.KnockoutMatches.Add(new KnockoutMatch
            {
                HomeTeamId = quarterFinals[2].Winner().Id,
                AwayTeamId = quarterFinals[3].Winner().Id,
                Type = KnockoutMatch.SEMIFINAL
            });
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GenerateKnockoutMatchesFinal()
        {
            var semiFinals = _db.KnockoutMatches.Include(k => k.HomeTeam).Include(k => k.AwayTeam).Where(x => x.Type == KnockoutMatch.SEMIFINAL).ToList();
            if(semiFinals.Count != 2) {
                return null;
            }
            _db.KnockoutMatches.Add(new KnockoutMatch
                {
                    HomeTeamId = semiFinals[0].Winner().Id,
                    AwayTeamId = semiFinals[1].Winner().Id,
                    Type = KnockoutMatch.FINAL
                });
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            var knockoutmatches = _db.KnockoutMatches.Include(k => k.HomeTeam).Include(k => k.AwayTeam);
            return View(knockoutmatches.ToList());
        }

        //
        // GET: /KnockoutMatch/Details/5

        public ActionResult Details(int id = 0)
        {
            KnockoutMatch knockoutmatch = _db.KnockoutMatches.Find(id);
            if (knockoutmatch == null)
            {
                return HttpNotFound();
            }
            return View(knockoutmatch);
        }

        //
        // GET: /KnockoutMatch/Create

        public ActionResult Create()
        {
            ViewBag.HomeTeamId = new SelectList(_db.Teams, "Id", "Name");
            ViewBag.AwayTeamId = new SelectList(_db.Teams, "Id", "Name");
            return View();
        }

        //
        // POST: /KnockoutMatch/Create

        [HttpPost]
        public ActionResult Create(KnockoutMatch knockoutmatch)
        {
            if (ModelState.IsValid)
            {
                _db.KnockoutMatches.Add(knockoutmatch);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HomeTeamId = new SelectList(_db.Teams, "Id", "Name", knockoutmatch.HomeTeamId);
            ViewBag.AwayTeamId = new SelectList(_db.Teams, "Id", "Name", knockoutmatch.AwayTeamId);
            return View(knockoutmatch);
        }

        //
        // GET: /KnockoutMatch/Edit/5
        [AllowAnonymous]
        public ActionResult Edit(int id = 0)
        {
            KnockoutMatch knockoutmatch = _db.KnockoutMatches.Find(id);
            if (knockoutmatch == null)
            {
                return HttpNotFound();
            }
            ViewBag.HomeTeamId = new SelectList(_db.Teams, "Id", "Name", knockoutmatch.HomeTeamId);
            ViewBag.AwayTeamId = new SelectList(_db.Teams, "Id", "Name", knockoutmatch.AwayTeamId);
            return View(knockoutmatch);
        }

        //
        // POST: /KnockoutMatch/Edit/5

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Edit(KnockoutMatch knockoutmatch)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(knockoutmatch).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Finaler", "Home");
            }
            ViewBag.HomeTeamId = new SelectList(_db.Teams, "Id", "Name", knockoutmatch.HomeTeamId);
            ViewBag.AwayTeamId = new SelectList(_db.Teams, "Id", "Name", knockoutmatch.AwayTeamId);
            return View(knockoutmatch);
        }

        //
        // GET: /KnockoutMatch/Delete/5

        public ActionResult Delete(int id = 0)
        {
            KnockoutMatch knockoutmatch = _db.KnockoutMatches.Find(id);
            if (knockoutmatch == null)
            {
                return HttpNotFound();
            }
            return View(knockoutmatch);
        }

        //
        // POST: /KnockoutMatch/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            KnockoutMatch knockoutmatch = _db.KnockoutMatches.Find(id);
            _db.KnockoutMatches.Remove(knockoutmatch);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }
    }
}