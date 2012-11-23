using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FG12.Models;
using System.Data.Entity;
using FG12.ViewModels;

namespace FG12.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _db = new DataContext();

        public ActionResult Index()
        {
            var teams = _db.Teams.Include(t => t.Group).ToList();
            var teamsByGroup = teams.Select(x => x.Group.Name).Distinct().Select(id => teams.Where(x => x.Group.Name == id).ToList()).ToList();
            var orderedTeams = new List<IEnumerable<Team>>();
            teamsByGroup.ForEach(t => orderedTeams.Add(Standing.SortTeams(t)));
            ViewBag.Groups = orderedTeams;
            var groupMatches = _db.Matches.Include(m => m.HomeTeam).Include(m => m.AwayTeam).ToList();
            var matchListViewModel = new MatchListViewModel
                {
                    ComingMatches =
                        groupMatches.Where(x => x.HomeTeamGoals == null || x.AwayTeamGoals == null).OrderBy(x => x.Id).
                            Take(8).ToList(),
                    PlayedMatches =
                        groupMatches.Where(x => x.HomeTeamGoals != null || x.AwayTeamGoals != null).OrderByDescending(
                            x => x.Id).ToList(),
                    AllMatches = groupMatches.ToList(),
                    Mode = Mode.Group
                };
            return View(matchListViewModel);
        }

        public ActionResult Mellomspill()
        {
            var teams = _db.Teams.Include(t => t.GroupMiddleStage).Where(x => x.GroupMiddleStageId != null).OrderBy(x => x.GroupId).ToList();
            var teamsByGroup = teams.Select(x => x.GroupMiddleStage.Name).Distinct().OrderBy(x => x).Select(id => teams.Where(x => x.GroupMiddleStage.Name == id).ToList()).ToList();
            var orderedTeams = new List<IEnumerable<Team>>();
            teamsByGroup.ForEach(t => orderedTeams.Add(Standing.SortTeamsMiddleStage(t)));
            ViewBag.Groups = orderedTeams;
            var matchesMiddleStage = _db.MatchesMiddleStage.Include(m => m.HomeTeam).Include(m => m.AwayTeam).ToList();
            var viewModel = new MatchListViewModel
                {
                    ComingMatches = 
                        matchesMiddleStage.Where(x => x.HomeTeamGoals == null || x.AwayTeamGoals == null).OrderBy(
                            x => x.Id).Take(8).ToList(),
                    PlayedMatches =
                        matchesMiddleStage.Where(x => x.HomeTeamGoals != null || x.AwayTeamGoals != null).
                            OrderByDescending(x => x.Id).ToList(),
                    AllMatches = matchesMiddleStage.ToList(),
                    Mode = Mode.MiddleStage
                };
            ViewBag.IsActivated = _db.MatchesMiddleStage.Any();
            return View(viewModel);
        }

        public ActionResult Finaler()
        {
            var knockoutMatches = _db.KnockoutMatches.ToList();
            ViewBag.IsQuarterFinalsActivated = knockoutMatches.Any(x => x.Type == KnockoutMatch.QUARTERFINAL);
            ViewBag.IsSemiFinalsActivated = knockoutMatches.Any(x => x.Type == KnockoutMatch.SEMIFINAL);
            ViewBag.IsFinalActivated = knockoutMatches.Any(x => x.Type == KnockoutMatch.FINAL);
            var matchListViewModel = new MatchListViewModel
                {
                    ComingMatches =
                        knockoutMatches.Where(x => x.HomeTeamGoals == null || x.AwayTeamGoals == null).OrderBy(x => x.Id)
                            .Take(8).ToList(),
                    PlayedMatches =
                        knockoutMatches.Where(x => x.HomeTeamGoals != null || x.AwayTeamGoals != null).OrderByDescending
                            (x => x.Id).ToList(),
                    AllMatches = knockoutMatches.OrderByDescending(x => x.Id).ToList(),
                    Mode = Mode.Knockout
                };
            return View(matchListViewModel);
        }

        public ActionResult Skader(int id, string hometeam, string awayteam)
        {
            return View(new SkaderViewModel(hometeam, awayteam));
        }
    }
}
