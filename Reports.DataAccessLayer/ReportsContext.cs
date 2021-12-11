using Microsoft.EntityFrameworkCore;
using Reports.DataAccessLayer.Entities;

namespace Reports.DataAccessLayer
{
    public class ReportsContext : DbContext
    {
        public DbSet<DbUser> Users { get; set; }

        public DbSet<DbTask> Tasks { get; set; }

        public DbSet<DbReport> Reports { get; set; }

        public DbSet<DbLogin> Logins { get; set; }

        public ReportsContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbTask>()
                .HasOne<DbUser>();

            modelBuilder.Entity<DbReport>()
                .HasOne<DbUser>();
        }
    }
}