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
    public class MatchMiddleStageController : Controller
    {
        private DataContext _db = new DataContext();

        //
        // GET: /MatchMiddleStage/

        public ActionResult Index()
        {
            var matchesmiddlestage = _db.MatchesMiddleStage.Include(m => m.HomeTeam).Include(m => m.AwayTeam);
            return View(matchesmiddlestage.ToList());
        }

        //
        // GET: /MatchMiddleStage/Details/5

        public ActionResult Details(int id = 0)
        {
            MatchMiddleStage matchmiddlestage = _db.MatchesMiddleStage.Find(id);
            if (matchmiddlestage == null) {
                return HttpNotFound();
            }
            return View(matchmiddlestage);
        }

        //
        // GET: /MatchMiddleStage/Create

        public ActionResult Create()
        {
            ViewBag.HomeTeamId = new SelectList(_db.Teams, "Id", "Name");
            ViewBag.AwayTeamId = new SelectList(_db.Teams, "Id", "Name");
            return View();
        }

        //
        // POST: /MatchMiddleStage/Create

        [HttpPost]
        public ActionResult Create(MatchMiddleStage matchmiddlestage)
        {
            var matchError = GetMatchError(matchmiddlestage);
            if (ModelState.IsValid && string.IsNullOrEmpty(matchError)) {
                _db.MatchesMiddleStage.Add(matchmiddlestage);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HomeTeamId = new SelectList(_db.Teams, "Id", "Name", matchmiddlestage.HomeTeamId);
            ViewBag.AwayTeamId = new SelectList(_db.Teams, "Id", "Name", matchmiddlestage.AwayTeamId);
            ViewBag.Error = matchError;
            return View(matchmiddlestage);
        }

        private string GetMatchError(MatchMiddleStage matchmiddlestage)
        {
            var homeTeam = _db.Teams.Find(matchmiddlestage.HomeTeamId);
            var awayTeam = _db.Teams.Find(matchmiddlestage.AwayTeamId);
            var isInSameGroup = homeTeam.GroupMiddleStageId == awayTeam.GroupMiddleStageId;
            var homeTeamOpponents =
                homeTeam.HomeMatchesMiddleStage.Select(x => x.AwayTeamId).Concat(
                    homeTeam.AwayMatchesMiddleStage.Select(x => x.HomeTeamId)).Distinct();
            var hasNotPlayedBefore = !homeTeamOpponents.Contains(awayTeam.Id);
            var error = isInSameGroup ? string.Empty : "Ikke i samme gruppe";
            error += hasNotPlayedBefore ? string.Empty : "Kamp allerede registrert";
            error += matchmiddlestage.HomeTeamId != matchmiddlestage.AwayTeamId ? string.Empty : "Samme lag";
            return error;
        }

        //
        // GET: /MatchMiddleStage/Edit/5
        [AllowAnonymous]
        public ActionResult Edit(int id = 0)
        {
            MatchMiddleStage matchmiddlestage = _db.MatchesMiddleStage.Find(id);
            if (matchmiddlestage == null) {
                return HttpNotFound();
            }
            ViewBag.HomeTeamId = new SelectList(_db.Teams, "Id", "Name", matchmiddlestage.HomeTeamId);
            ViewBag.AwayTeamId = new SelectList(_db.Teams, "Id", "Name", matchmiddlestage.AwayTeamId);
            return View(matchmiddlestage);
        }

        //
        // POST: /MatchMiddleStage/Edit/5

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Edit(MatchMiddleStage matchmiddlestage)
        {
            if (ModelState.IsValid) {
                _db.Entry(matchmiddlestage).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Mellomspill", "Home");
            }
            ViewBag.HomeTeamId = new SelectList(_db.Teams, "Id", "Name", matchmiddlestage.HomeTeamId);
            ViewBag.AwayTeamId = new SelectList(_db.Teams, "Id", "Name", matchmiddlestage.AwayTeamId);
            return View(matchmiddlestage);
        }

        //
        // GET: /MatchMiddleStage/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MatchMiddleStage matchmiddlestage = _db.MatchesMiddleStage.Find(id);
            _db.MatchesMiddleStage.Remove(matchmiddlestage);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult GenerateMiddleMatchGroups()
        {
            var teams = _db.Teams.Include(t => t.HomeMatches).Include(t => t.AwayMatches).Include(t => t.Group).ToList();
            if (teams.Any(x => x.Matches.Count < 3)) {
                // all teams three matches.
                return null;
            }
            var teamsByGroup = teams.Select(x => x.Group.Name).Distinct().OrderBy(x => x).Select(id => teams.Where(x => x.Group.Name == id).ToList()).ToList();
            var orderedTeams = new List<List<Team>>();
            teamsByGroup.ForEach(t => orderedTeams.Add(Standing.SortTeams(t.ToList())));
            // Group M1
            var teamA1 = orderedTeams[0][0].Id;
            var teamB2 = orderedTeams[1][1].Id;
            var teamC3 = orderedTeams[2][2].Id;
            var teamD4 = orderedTeams[3][3].Id;
            _db.MatchesMiddleStage.Add(new MatchMiddleStage { Id = 1, HomeTeamId = teamA1, AwayTeamId = teamB2 });
            _db.MatchesMiddleStage.Add(new MatchMiddleStage {Id = 2, HomeTeamId = teamC3, AwayTeamId = teamD4});
            _db.MatchesMiddleStage.Add(new MatchMiddleStage { Id = 9, HomeTeamId = teamB2, AwayTeamId = teamC3 });
            _db.MatchesMiddleStage.Add(new MatchMiddleStage { Id = 10, HomeTeamId = teamD4, AwayTeamId = teamA1 });
            _db.MatchesMiddleStage.Add(new MatchMiddleStage { Id = 17, HomeTeamId = teamA1, AwayTeamId = teamC3 });
            _db.MatchesMiddleStage.Add(new MatchMiddleStage { Id = 18, HomeTeamId = teamD4, AwayTeamId = teamB2 });

            // Group M2
            var teamB1 = orderedTeams[1][0].Id;
            var teamC2 = orderedTeams[2][1].Id;
            var teamD3 = orderedTeams[3][2].Id;
            var teamA4 = orderedTeams[0][3].Id;
            _db.MatchesMiddleStage.Add(new MatchMiddleStage {Id = 3, HomeTeamId = teamB1, AwayTeamId = teamC2});
            _db.MatchesMiddleStage.Add(new MatchMiddleStage {Id = 4, HomeTeamId = teamD3, AwayTeamId = teamA4});
            _db.MatchesMiddleStage.Add(new MatchMiddleStage { Id = 11, HomeTeamId = teamC2, AwayTeamId = teamD3 });
            _db.MatchesMiddleStage.Add(new MatchMiddleStage { Id = 12, HomeTeamId = teamA4, AwayTeamId = teamB1 });
            _db.MatchesMiddleStage.Add(new MatchMiddleStage { Id = 19, HomeTeamId = teamB1, AwayTeamId = teamD3 });
            _db.MatchesMiddleStage.Add(new MatchMiddleStage { Id = 20, HomeTeamId = teamA4, AwayTeamId = teamC2 });

            // Group M3
            var teamC1 = orderedTeams[2][0].Id;
            var teamD2 = orderedTeams[3][1].Id;
            var teamA3 = orderedTeams[0][2].Id;
            var teamB4 = orderedTeams[1][3].Id;
            _db.MatchesMiddleStage.Add(new MatchMiddleStage {Id = 5, HomeTeamId = teamC1, AwayTeamId = teamD2});
            _db.MatchesMiddleStage.Add(new MatchMiddleStage {Id = 6, HomeTeamId = teamA3, AwayTeamId = teamB4});
            _db.MatchesMiddleStage.Add(new MatchMiddleStage { Id = 13, HomeTeamId = teamD2, AwayTeamId = teamA3 });
            _db.MatchesMiddleStage.Add(new MatchMiddleStage { Id = 14, HomeTeamId = teamB4, AwayTeamId = teamC1 });
            _db.MatchesMiddleStage.Add(new MatchMiddleStage { Id = 21, HomeTeamId = teamC1, AwayTeamId = teamA3 });
            _db.MatchesMiddleStage.Add(new MatchMiddleStage { Id = 22, HomeTeamId = teamB4, AwayTeamId = teamD2 });

            // Group M4
            var teamD1 = orderedTeams[3][0].Id;
            var teamA2 = orderedTeams[0][1].Id;
            var teamB3 = orderedTeams[1][2].Id;
            var teamC4 = orderedTeams[2][3].Id;
            _db.MatchesMiddleStage.Add(new MatchMiddleStage {Id = 7, HomeTeamId = teamD1, AwayTeamId = teamA2});
            _db.MatchesMiddleStage.Add(new MatchMiddleStage {Id = 8, HomeTeamId = teamB3, AwayTeamId = teamC4});
            _db.MatchesMiddleStage.Add(new MatchMiddleStage {Id = 15, HomeTeamId = teamA2, AwayTeamId = teamB3});
            _db.MatchesMiddleStage.Add(new MatchMiddleStage {Id = 16, HomeTeamId = teamC4, AwayTeamId = teamD1});
            _db.MatchesMiddleStage.Add(new MatchMiddleStage {Id = 23, HomeTeamId = teamD1, AwayTeamId = teamB3});
            _db.MatchesMiddleStage.Add(new MatchMiddleStage {Id = 24, HomeTeamId = teamC4, AwayTeamId = teamA2});

            const int point1 = 3;
            const int point2 = 2;
            const int point3 = 1;
            const int point4 = 0;
            _db.Teams.Find(teamA1).GroupMiddleStageId = 5;
            _db.Teams.Find(teamA1).AdditionalPointsFromGroupStage = point1;
            _db.Teams.Find(teamB2).GroupMiddleStageId = 5;
            _db.Teams.Find(teamB2).AdditionalPointsFromGroupStage = point2;
            _db.Teams.Find(teamC3).GroupMiddleStageId = 5;
            _db.Teams.Find(teamC3).AdditionalPointsFromGroupStage = point3;
            _db.Teams.Find(teamD4).GroupMiddleStageId = 5;
            _db.Teams.Find(teamD4).AdditionalPointsFromGroupStage = point4;
            _db.Teams.Find(teamB1).GroupMiddleStageId = 6;
            _db.Teams.Find(teamB1).AdditionalPointsFromGroupStage = point1;
            _db.Teams.Find(teamC2).GroupMiddleStageId = 6;
            _db.Teams.Find(teamC2).AdditionalPointsFromGroupStage = point2;
            _db.Teams.Find(teamD3).GroupMiddleStageId = 6;
            _db.Teams.Find(teamD3).AdditionalPointsFromGroupStage = point3;
            _db.Teams.Find(teamA4).GroupMiddleStageId = 6;
            _db.Teams.Find(teamA4).AdditionalPointsFromGroupStage = point4;
            _db.Teams.Find(teamC1).GroupMiddleStageId = 7;
            _db.Teams.Find(teamC1).AdditionalPointsFromGroupStage = point1;
            _db.Teams.Find(teamD2).GroupMiddleStageId = 7;
            _db.Teams.Find(teamD2).AdditionalPointsFromGroupStage = point2;
            _db.Teams.Find(teamA3).GroupMiddleStageId = 7;
            _db.Teams.Find(teamA3).AdditionalPointsFromGroupStage = point3;
            _db.Teams.Find(teamB4).GroupMiddleStageId = 7;
            _db.Teams.Find(teamB4).AdditionalPointsFromGroupStage = point4;
            _db.Teams.Find(teamD1).GroupMiddleStageId = 8;
            _db.Teams.Find(teamD1).AdditionalPointsFromGroupStage = point1;
            _db.Teams.Find(teamA2).GroupMiddleStageId = 8;
            _db.Teams.Find(teamA2).AdditionalPointsFromGroupStage = point2;
            _db.Teams.Find(teamB3).GroupMiddleStageId = 8;
            _db.Teams.Find(teamB3).AdditionalPointsFromGroupStage = point3;
            _db.Teams.Find(teamC4).GroupMiddleStageId = 8;
            _db.Teams.Find(teamC4).AdditionalPointsFromGroupStage = point4;
            _db.SaveChanges();
            return RedirectToAction("Mellomspill", "Home");
        }
    }
}