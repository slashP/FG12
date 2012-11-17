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
                new Group {Id = 1, Name = "A"},
                new Group {Id = 2, Name = "B"},
                new Group {Id = 3, Name = "C"},
                new Group {Id = 4, Name = "D"},
                new Group {Id = 5, Name = "A2"},
                new Group {Id = 6, Name = "B2"},
                new Group {Id = 7, Name = "C2"},
                new Group {Id = 8, Name = "D2"}
            );

            context.Teams.AddOrUpdate(
                new Team { Id = 1, Name = "Atletico Madrid", GroupId = 1, GroupMiddleStageId = 5 },
                new Team { Id = 2, Name = "AC Milan", GroupId = 1},
                new Team { Id = 3, Name = "Napoli", GroupId = 1},
                new Team { Id = 4, Name = "Lyon", GroupId = 1, GroupMiddleStageId = 5 },
                new Team { Id = 5, Name = "FC Porto", GroupId = 2},
                new Team { Id = 6, Name = "AS Roma", GroupId = 2},
                new Team { Id = 7, Name = "Sevilla", GroupId = 2, GroupMiddleStageId = 5 },
                new Team { Id = 8, Name = "Arsenal", GroupId = 2, GroupMiddleStageId = 5 },
                new Team { Id = 9, Name = "Bordeaux", GroupId = 3},
                new Team { Id = 10, Name = "Liverpool", GroupId = 3},
                new Team { Id = 11, Name = "Valencia", GroupId = 3},
                new Team { Id = 12, Name = "Juventus", GroupId = 3}

            );
        }
    }
}
