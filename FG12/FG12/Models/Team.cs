using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;

namespace FG12.Models
{
    public class Team
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public int GroupId { get; set; }
        public virtual Group Group { get; set; }

        public virtual ICollection<Match> HomeMatches { get; set; }
        public virtual ICollection<Match> AwayMatches { get; set; }


        public int? GroupMiddleStageId { get; set; }
        public virtual Group GroupMiddleStage { get; set; }
        public virtual ICollection<MatchMiddleStage> HomeMatchesMiddleStage { get; set; }
        public virtual ICollection<MatchMiddleStage> AwayMatchesMiddleStage { get; set; }

        public virtual ICollection<KnockoutMatch> KnockoutHomeMatches { get; set; }
        public virtual ICollection<KnockoutMatch> KnockoutAwayMatches { get; set; }

        public int? AdditionalPointsFromGroupStage { get; set; }

        [NotMapped]
        [ScaffoldColumn(false)]
        public Collection<Match> Matches
        {
            get
            {
                if (HomeMatches == null)
                {
                    HomeMatches = new Collection<Match>();
                }
                if(AwayMatches == null)
                {
                    AwayMatches = new Collection<Match>();
                }
                return new Collection<Match>(HomeMatches.Concat(AwayMatches).OrderBy(m => m.Id).ToList());
            }
            set { Debug.WriteLine("setter called  " + value); }
        }

        [NotMapped]
        [ScaffoldColumn(false)]
        public Collection<Match> PlayedMatches
        {
            get
            {
                if (HomeMatches == null)
                {
                    HomeMatches = new Collection<Match>();
                }
                if (AwayMatches == null)
                {
                    AwayMatches = new Collection<Match>();
                }
                return new Collection<Match>(HomeMatches.Concat(AwayMatches).Where(m => m.AwayTeamGoals != null && m.HomeTeamGoals != null).ToList());
            }
            set { Debug.WriteLine("setter called  " + value); }
        }

        [NotMapped]
        [ScaffoldColumn(false)]
        public int GoalsScored
        {
            get
            {
                var sum = 0;
                if (HomeMatches != null)
                {
                    sum += HomeMatches.Sum(homeMatch => homeMatch.HomeTeamGoals ?? 0);
                }
                if (AwayMatches != null)
                {
                    sum += AwayMatches.Sum(awayMatch => awayMatch.AwayTeamGoals ?? 0);
                }
                return sum;
            }
            set { Debug.WriteLine("setter called  " + value); }
        }

        [NotMapped]
        [ScaffoldColumn(false)]
        public int GoalsConceded
        {
            get
            {
                var sum = 0;
                if(HomeMatches != null)
                {
                    sum += HomeMatches.Sum(homeMatch => homeMatch.AwayTeamGoals ?? 0);
                }
                if(AwayMatches != null)
                {
                    sum += AwayMatches.Sum(awayMatch => awayMatch.HomeTeamGoals ?? 0);
                }
                return sum;
            }
            set { Debug.WriteLine("setter called  " + value); }
        }

        [NotMapped]
        [ScaffoldColumn(false)]
        public int GoalDifference { get { return GoalsScored - GoalsConceded; } }

        [NotMapped]
        [ScaffoldColumn(false)]
        public int Points
        {
            get
            {
                var sum = 0;
                if (HomeMatches != null)
                {
                    foreach (var homeMatch in HomeMatches.Where(homeMatch => homeMatch.HomeTeamGoals != null))
                    {
                        if (homeMatch.HomeTeamGoals == homeMatch.AwayTeamGoals)
                            sum += 1;
                        else if (homeMatch.HomeTeamGoals > homeMatch.AwayTeamGoals)
                            sum += 3;
                    }
                }
                if (AwayMatches != null)
                {
                    foreach (var awayMatch in AwayMatches.Where(awayMatch => awayMatch.HomeTeamGoals != null))
                    {
                        if (awayMatch.HomeTeamGoals == awayMatch.AwayTeamGoals)
                            sum += 1;
                        else if (awayMatch.AwayTeamGoals > awayMatch.HomeTeamGoals)
                            sum += 3;
                    }
                }
                return sum;
            }
            set { Debug.WriteLine("setter called  " + value); }
        }

        [NotMapped]
        [ScaffoldColumn(false)]
        public Collection<MatchMiddleStage> PlayedMatchesMiddleStage
        {
            get
            {
                if (HomeMatchesMiddleStage == null)
                {
                    HomeMatchesMiddleStage = new Collection<MatchMiddleStage>();
                }
                if (AwayMatchesMiddleStage == null)
                {
                    AwayMatchesMiddleStage = new Collection<MatchMiddleStage>();
                }
                return new Collection<MatchMiddleStage>(HomeMatchesMiddleStage.Concat(AwayMatchesMiddleStage).Where(m => m.AwayTeamGoals != null && m.HomeTeamGoals != null).ToList());
            }
            set { Debug.WriteLine("setter called  " + value); }
        }

        [NotMapped]
        [ScaffoldColumn(false)]
        public int GoalsScoredMiddleStage
        {
            get
            {
                var sum = 0;
                if (HomeMatchesMiddleStage != null)
                {
                    sum += HomeMatchesMiddleStage.Sum(homeMatch => homeMatch.HomeTeamGoals ?? 0);
                }
                if (AwayMatchesMiddleStage != null)
                {
                    sum += AwayMatchesMiddleStage.Sum(awayMatch => awayMatch.AwayTeamGoals ?? 0);
                }
                return sum;
            }
            set { Debug.WriteLine("setter called  " + value); }
        }

        [NotMapped]
        [ScaffoldColumn(false)]
        public int GoalsConcededMiddleStage
        {
            get
            {
                var sum = 0;
                if (HomeMatchesMiddleStage != null)
                {
                    sum += HomeMatchesMiddleStage.Sum(homeMatch => homeMatch.AwayTeamGoals ?? 0);
                }
                if (AwayMatchesMiddleStage != null)
                {
                    sum += AwayMatchesMiddleStage.Sum(awayMatch => awayMatch.HomeTeamGoals ?? 0);
                }
                return sum;
            }
            set { Debug.WriteLine("setter called  " + value); }
        }

        [NotMapped]
        [ScaffoldColumn(false)]
        public int GoalDifferenceMiddleStage { get { return GoalsScoredMiddleStage - GoalsConcededMiddleStage; } }

        [NotMapped]
        [ScaffoldColumn(false)]
        public int PointsMiddleStage
        {
            get
            {
                var sum = 0;
                if (HomeMatchesMiddleStage != null)
                {
                    foreach (var homeMatch in HomeMatchesMiddleStage.Where(homeMatch => homeMatch.HomeTeamGoals != null))
                    {
                        if (homeMatch.HomeTeamGoals == homeMatch.AwayTeamGoals)
                            sum += 1;
                        else if (homeMatch.HomeTeamGoals > homeMatch.AwayTeamGoals)
                            sum += 3;
                    }
                }
                if (AwayMatchesMiddleStage != null)
                {
                    foreach (var awayMatch in AwayMatchesMiddleStage.Where(awayMatch => awayMatch.HomeTeamGoals != null))
                    {
                        if (awayMatch.HomeTeamGoals == awayMatch.AwayTeamGoals)
                            sum += 1;
                        else if (awayMatch.AwayTeamGoals > awayMatch.HomeTeamGoals)
                            sum += 3;
                    }
                }
                return sum + AdditionalPointsFromGroupStage ?? 0;
            }
            set { Debug.WriteLine("setter called  " + value); }
        }
    }
}