using Microsoft.EntityFrameworkCore;

namespace ViewModel.ServiceModel
{
    public class ServiceContext : DbContext
    {
        public ServiceContext(DbContextOptions<ServiceContext> options): base(options) { }

        public DbSet<HRDept> HRDept { get; set; }
        public DbSet<HRGrade> HRGrade { get; set; }
        public DbSet<HRUsers> HRUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
