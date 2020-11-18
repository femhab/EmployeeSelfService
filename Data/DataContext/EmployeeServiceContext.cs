using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.DataContext
{
    public class EmployeeServiceContext: IdentityDbContext<AppIdentityUser>
    {
        public EmployeeServiceContext(DbContextOptions<EmployeeServiceContext> options) : base(options) { }

        public DbSet<ApprovalBoard> ApprovalBoards { get; set; }
        public DbSet<ApprovalSetting> ApprovalSettings { get; set; }
        public DbSet<ApprovalWorkItem> ApprovalWorkItems { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeAddress> EmployeesAddress { get; set; }
        public DbSet<EmployeeApprovalConfig> EmployeesApprovalConfig { get; set; }
        public DbSet<EmployeeEducationDetail> EmployeesEducationDetail { get; set; }
        public DbSet<GradeLevel> GradeLevels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
