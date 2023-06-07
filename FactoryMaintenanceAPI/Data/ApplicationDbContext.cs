using FactoryMaintenanceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FactoryMaintenanceAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //Add models
        public DbSet<Country> Countries { get; set; }
        public DbSet<Factory> Factories { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<MaintenanceChore> MaintenanceChores { get; set; }
    }
}
