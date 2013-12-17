using FG12.Models;

namespace FG12.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FG12.Models.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Models.DataContext context)
        {
            context.Groups.AddOrUpdate(
                new Group {Id = 1, Name = "1"},
                new Group {Id = 2, Name = "2"},
                new Group {Id = 3, Name = "3"},
                new Group {Id = 4, Name = "4"},
                new Group {Id = 5, Name = "M1"},
                new Group {Id = 6, Name = "M2"},
                new Group {Id = 7, Name = "M3"},
                new Group {Id = 8, Name = "M4"}
            );

            context.Teams.AddOrUpdate(
                new Team { Id = 1, Name = "Nederland", GroupId = 1 },
                new Team { Id = 2, Name = "Frankrike", GroupId = 1 },
                new Team { Id = 3, Name = "Sverige", GroupId = 1 },
                new Team { Id = 4, Name = "England", GroupId = 1 },
                new Team { Id = 5, Name = "Belgia", GroupId = 2 },
                new Team { Id = 6, Name = "Chile", GroupId = 2 },
                new Team { Id = 7, Name = "Polen", GroupId = 2 },
                new Team { Id = 8, Name = "Portugal", GroupId = 2 },
                new Team { Id = 9, Name = "Italia", GroupId = 3 },
                new Team { Id = 10, Name = "Uruguay", GroupId = 3 },
                new Team { Id = 11, Name = "Colombia", GroupId = 3 },
                new Team { Id = 12, Name = "Elfenbenskysten", GroupId = 3 },
                new Team { Id = 13, Name = "Spania", GroupId = 4 },
                new Team { Id = 14, Name = "Argentina", GroupId = 4 },
                new Team { Id = 15, Name = "Brasil", GroupId = 4 },
                new Team { Id = 16, Name = "Tyskland", GroupId = 4 }
            );

            context.Matches.AddOrUpdate(
                // Round 1
                new Match { Id = 1, HomeTeamId = 1, AwayTeamId = 2 },
                new Match { Id = 2, HomeTeamId = 3, AwayTeamId = 4 },
                new Match { Id = 3, HomeTeamId = 5, AwayTeamId = 6 },
                new Match { Id = 4, HomeTeamId = 7, AwayTeamId = 8 },
                new Match { Id = 5, HomeTeamId = 9, AwayTeamId = 10 },
                new Match { Id = 6, HomeTeamId = 11, AwayTeamId = 12 },
                new Match { Id = 7, HomeTeamId = 13, AwayTeamId = 14 },
                new Match { Id = 8, HomeTeamId = 15, AwayTeamId = 16 },

                // Round 2
                new Match { Id = 9, HomeTeamId = 2, AwayTeamId = 3 },
                new Match { Id = 10, HomeTeamId = 4, AwayTeamId = 1 },
                new Match { Id = 11, HomeTeamId = 6, AwayTeamId = 7 },
                new Match { Id = 12, HomeTeamId = 8, AwayTeamId = 5 },
                new Match { Id = 13, HomeTeamId = 10, AwayTeamId = 11 },
                new Match { Id = 14, HomeTeamId = 12, AwayTeamId = 9 },
                new Match { Id = 15, HomeTeamId = 14, AwayTeamId = 15 },
                new Match { Id = 16, HomeTeamId = 16, AwayTeamId = 13 },
                
                // Round 3
                new Match { Id = 17, HomeTeamId = 1, AwayTeamId = 3 },
                new Match { Id = 18, HomeTeamId = 4, AwayTeamId = 2 },
                new Match { Id = 19, HomeTeamId = 5, AwayTeamId = 7 },
                new Match { Id = 20, HomeTeamId = 8, AwayTeamId = 6 },
                new Match { Id = 21, HomeTeamId = 9, AwayTeamId = 11 },
                new Match { Id = 22, HomeTeamId = 12, AwayTeamId = 10 },
                new Match { Id = 23, HomeTeamId = 13, AwayTeamId = 15 },
                new Match { Id = 24, HomeTeamId = 16, AwayTeamId = 14 }
            );
        }
    }
}
