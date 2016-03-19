using System.Data.Entity;
using EmployeeDirectory.Web.Models.Sqlite;
using EmployeeDirectory.Web.Models.SqlServer;

namespace EmployeeDirectory.Web.Models
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext() : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // The correct initializer will be used based on the providerName for the
            // DefaultConnection connectionString.
            Database.SetInitializer(new EmployeeDbSqlServerInitializer());
            Database.SetInitializer(new EmployeeDbSqliteInitializer(modelBuilder));
        }

        public virtual DbSet<Employee> Employees { get; set; }
    }
}