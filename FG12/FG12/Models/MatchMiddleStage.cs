using System;

namespace FG12.Models
{
    public class MatchMiddleStage : IMatch
    {
        public int Id { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }

        public virtual Team HomeTeam { get; set; }
        public virtual Team AwayTeam { get; set; }
        public int? HomeTeamGoals { get; set; }
        public int? AwayTeamGoals { get; set; }
        public string HomeTeamName { get { return HomeTeam.Name; } }
        public string AwayTeamName { get { return AwayTeam.Name; } }
    }
}