using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EuroApi.Models;
using FG12.Models;

namespace FG12.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _db = new DataContext();

        public ActionResult Index()
        {
            var teams = _db.Teams.Include("Group").ToList();
            var teamsByGroup = teams.Select(x => x.Group.Name).Distinct().Select(id => teams.Where(x => x.Group.Name == id).ToList()).ToList();
            var orderedTeams = new List<IEnumerable<Team>>();
            teamsByGroup.ForEach(t => orderedTeams.Add(Standing.SortTeams(t)));
            ViewBag.Groups = orderedTeams;
            return View();
        }

        public ActionResult Mellomspill()
        {
            var teams = _db.Teams.Include("GroupMiddleStage").Where(x => x.GroupMiddleStageId != null).ToList();
            var teamsByGroup = teams.Select(x => x.GroupMiddleStage.Name).Distinct().Select(id => teams.Where(x => x.GroupMiddleStage.Name == id).ToList()).ToList();
            var orderedTeams = new List<IEnumerable<Team>>();
            teamsByGroup.ForEach(t => orderedTeams.Add(Standing.SortTeamsMiddleStage(t)));
            ViewBag.Groups = orderedTeams;
            return View();
        }

        public ActionResult Finaler()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
