using P02_FootballBetting.Common;
using P02_FootballBetting.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace P02_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {

        public FootballBettingContext()
        {

        }
        public FootballBettingContext(DbContextOptions<FootballBettingContext> options)
            : base(options)
        {
            if (this.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                this.Database.Migrate();
            }
        }

        public virtual DbSet<Color> Colors { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Position> Positions { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Town> Towns { get; set; }

        public virtual DbSet<Team> Teams { get; set; }

        public virtual DbSet<Player> Players { get; set; }

        public virtual DbSet<Bet> Bets { get; set; }

        public virtual DbSet<Game> Games { get; set; }

        public virtual DbSet<PlayerStatistic> PlayersStatistics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerStatistic>()
                .HasKey(ps => new { ps.GameId, ps.PlayerId });
            modelBuilder.Entity<PlayerStatistic>()
                .HasOne(ps => ps.Game)
                .WithMany(g => g.PlayersStatistics);
            modelBuilder.Entity<PlayerStatistic>()
              .HasOne(ps => ps.Player)
              .WithMany(p => p.PlayersStatistics);

            modelBuilder.Entity<Team>()
                .HasOne(t => t.PrimaryKitColor)
                .WithMany(c => c.PrimaryKitTeams)
                .HasForeignKey(t => t.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
               .HasOne(t => t.SecondaryKitColor)
               .WithMany(c => c.SecondaryKitTeams)
               .HasForeignKey(t => t.SecondaryKitColorId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
             .HasOne(g => g.HomeTeam)
             .WithMany(ht => ht.HomeGames)
             .HasForeignKey(g => g.HomeTeamId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
             .HasOne(g => g.AwayTeam)
             .WithMany(at => at.AwayGames)
             .HasForeignKey(g => g.AwayTeamId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.TeamId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
