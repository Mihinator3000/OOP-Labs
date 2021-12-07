using Microsoft.EntityFrameworkCore;
using Reports.DataAccessLayer.Entities;

namespace Reports.DataAccessLayer
{
    public class ReportsContext : DbContext
    {
        public DbSet<DbUser> Users { get; set; }

        public DbSet<DbTask> Tasks { get; set; }

        public ReportsContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}