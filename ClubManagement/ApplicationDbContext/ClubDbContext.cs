using ClubManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubManagement.ApplicationDbContext
{
    public class ClubDbContext : DbContext
    {
        public ClubDbContext(DbContextOptions<ClubDbContext> options) : base(options) { }

        // Tabele
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Footballer> Footballers { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<GroupTraining> GroupTraining { get; set; }
        public DbSet<IndividualTraining> IndividualTraining { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Statistics> Statistics { get; set; }
        public DbSet<Match> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*Encje do konta*/
            // Account 1-1 Footballer
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Footballer)
                .WithOne(f => f.Account)
                .HasForeignKey<Footballer>(f => f.AccountId)
                .OnDelete(DeleteBehavior.SetNull);

            // Account 1-1 Coach
            modelBuilder.Entity<Account>()
                .HasOne(a => a.Coach)
                .WithOne(c => c.Account)
                .HasForeignKey<Coach>(c => c.AccountId)
                .OnDelete(DeleteBehavior.SetNull);

            /*Encje do piłkarzy*/

            // Statistics 1-1 Footballer
            /*modelBuilder.Entity<Statistics>()
                .HasOne(s => s.Footballer)
                .WithOne(f => f.Statistics)
                .HasForeignKey<Footballer>(s => s.StatisticsId);*/
            // ta wersja jest co nie ma połączenia przez id z pilkarzem
            modelBuilder.Entity<Footballer>()
                .HasOne(f => f.Statistics)
                .WithOne(s => s.Footballer)
                .HasForeignKey<Statistics>(s => s.FootballerId)
                .OnDelete(DeleteBehavior.Cascade);// Kasuje rekordy Statistics po usunięciu Footballer

            // Footballer 1-* IndividualTraining
            modelBuilder.Entity<IndividualTraining>()
                .HasOne(it => it.Footballer)
                .WithMany(f => f.IndividualTrainings)
                .HasForeignKey(it => it.FootballerId);

            // Footballer *-* GroupTraining  
            modelBuilder.Entity<GroupTraining>()
                .HasMany(gt => gt.Footballers)
                .WithMany(f => f.GroupTrainings)
                .UsingEntity(gt => gt.ToTable("FootballerGroupTraining"));

            // Footballer *-* Match
            modelBuilder.Entity<Match>()
                .HasMany(m => m.Footballers)
                .WithMany(f => f.Matches)
                .UsingEntity(m => m.ToTable("FootballerMatch"));

            /*Encje do trenerów*/

            // Coach 1-* IndividualTraining
            modelBuilder.Entity<IndividualTraining>()
                .HasOne(it => it.Coach)
                .WithMany(c => c.IndividualTrainings)
                .HasForeignKey(it => it.CoachId);

            // Footballer *-* GroupTraining  
            modelBuilder.Entity<GroupTraining>()
                .HasMany(gt => gt.Coaches)
                .WithMany(c => c.GroupTrainings)
                .UsingEntity(gt => gt.ToTable("CoachGroupTraining"));

            // Footballer *-* Match
            modelBuilder.Entity<Match>()
                .HasMany(m => m.Coaches)
                .WithMany(c => c.Matches)
                .UsingEntity(m => m.ToTable("CoachMatch"));
        }
    }
}
