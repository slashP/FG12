using System;
using System.ComponentModel.DataAnnotations;

namespace FG12.Models
{
    public class KnockoutMatch : IMatch
    {
        public const int QUARTERFINAL = 1;
        public const int SEMIFINAL = 2;
        public const int FINAL = 3;
        public int Id { get; set; }

        public int Type { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }

        public virtual Team HomeTeam { get; set; }
        public virtual Team AwayTeam { get; set; }
        public int? HomeTeamGoals { get; set; }
        public int? AwayTeamGoals { get; set; }
        public string HomeTeamName { get { return HomeTeam.Name; } }
        public string AwayTeamName { get { return AwayTeam.Name; } }

        public Team Winner()
        {
            if (HomeTeamGoals > AwayTeamGoals)
                return HomeTeam;
            return AwayTeamGoals > HomeTeamGoals ? AwayTeam : null;
        }
    }
}