using ClubManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ClubManagement.ApplicationDbContext
{
    public class ClubDbContext : DbContext
    {
        public ClubDbContext(DbContextOptions<ClubDbContext> options) : base(options) { }

        // Tabele
        //public DbSet<Role> Roles { get; set; } // Rola w innym miejscu
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
            // Encje do konta
            // Account *-1 Role
            /*modelBuilder.Entity<Account>()
                .HasOne(r => r.Role)
                .WithMany(a => a.Accounts)
                .HasForeignKey(r => r.RoleId);*/

            /*Encje do piłkarzy*/
            // Statistics 1-1 Footballer
            modelBuilder.Entity<Statistics>()
                .HasOne(s => s.Footballer)
                .WithOne(f => f.Statistics)
                .HasForeignKey<Footballer>(s => s.StatisticsId);

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

            // Encje do ról
            modelBuilder.Entity<Statistics>();

            // Encje do ról
            modelBuilder.Entity<Match>();

            // Encje do ról
            modelBuilder.Entity<Footballer>();

                
        }
    }
}
