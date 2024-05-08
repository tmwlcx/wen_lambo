using Microsoft.EntityFrameworkCore;
using AttorneyScheduler.DAL.Tables;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
namespace AttorneyScheduler.DAL
{
    public class AttorneySchedulerDbContext : DbContext
    {
        public AttorneySchedulerDbContext(DbContextOptions<AttorneySchedulerDbContext> options): base(options)
        {
        }

        public DbSet<Attorney> Attorney { get; set; }
        public DbSet<AttorneyType> AttorneyType { get; set; }

        public DbSet<AttorneyTimeOff> AttorneyTimeOff { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=DAL\\Data\\AttorneyScheduler.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attorney>()
                .HasKey(b => b.AttorneyId);

            modelBuilder.Entity<Attorney>()
                .HasOne(b => b.AttorneyType)
                .WithOne()
                .HasForeignKey<AttorneyType>(b => b.AttorneyTypeId);

            modelBuilder.Entity<Attorney>()
                .Property(b => b.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Attorney>()
                .Property(b => b.UpdatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<AttorneyType>()
                .HasKey(t => t.AttorneyTypeId);

            modelBuilder.Entity<AttorneyType>()
                .Property(b => b.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<AttorneyType>()
                .Property(b => b.UpdatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");



            modelBuilder.Entity<AttorneyTimeOff>()
                .HasKey(t => t.AttorneyTimeOffId);

            modelBuilder.Entity<AttorneyTimeOff>()
                .HasOne(a => a.Attorney)
                .WithMany(attorney => attorney.AttorneyTimeOff)
                .HasForeignKey(a => a.AttorneyId)
                .IsRequired();

        }

    }

}
