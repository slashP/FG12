using System.Collections.Generic;
using System.Linq;

namespace FG12.Models
{
    public class Standing
    {
        public static List<Team> SortTeams(List<Team> teams)
        {
            var sorted = teams.OrderByDescending(x => x.Points).ThenByDescending(x => x.GoalDifference).ThenByDescending(x => x.GoalsScored).ThenBy(x => x.Name).ToList();
            return sorted;
        }

        public static List<Team> SortTeamsMiddleStage(List<Team> teams)
        {
            var sorted = teams.OrderByDescending(x => x.PointsMiddleStage).ThenByDescending(x => x.GoalDifferenceMiddleStage).ThenByDescending(x => x.GoalsScoredMiddleStage).ThenBy(x => x.Name).ToList();
            return sorted;
        }
    }
}