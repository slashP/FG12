using System.Data.Entity;

namespace FG12.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DataContext") { }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Team> Teams { get; set; }

        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchMiddleStage> MatchesMiddleStage { get; set; }

        public DbSet<KnockoutMatch> KnockoutMatches { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>()
                        .HasRequired(m => m.HomeTeam)
                        .WithMany(t => t.HomeMatches)
                        .HasForeignKey(m => m.HomeTeamId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Match>()
                        .HasRequired(m => m.AwayTeam)
                        .WithMany(t => t.AwayMatches)
                        .HasForeignKey(m => m.AwayTeamId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<MatchMiddleStage>()
                        .HasRequired(m => m.HomeTeam)
                        .WithMany(t => t.HomeMatchesMiddleStage)
                        .HasForeignKey(m => m.HomeTeamId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<MatchMiddleStage>()
                        .HasRequired(m => m.AwayTeam)
                        .WithMany(t => t.AwayMatchesMiddleStage)
                        .HasForeignKey(m => m.AwayTeamId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<KnockoutMatch>()
                        .HasRequired(m => m.HomeTeam)
                        .WithMany(t => t.KnockoutHomeMatches)
                        .HasForeignKey(m => m.HomeTeamId)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<KnockoutMatch>()
                        .HasRequired(m => m.AwayTeam)
                        .WithMany(t => t.KnockoutAwayMatches)
                        .HasForeignKey(m => m.AwayTeamId)
                        .WillCascadeOnDelete(false);
        }
    }
}