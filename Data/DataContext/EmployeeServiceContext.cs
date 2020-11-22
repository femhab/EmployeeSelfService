using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.DataContext
{
    public class EmployeeServiceContext: IdentityDbContext<AppIdentityUser>
    {
        public EmployeeServiceContext(DbContextOptions<EmployeeServiceContext> options) : base(options) { }

        public DbSet<AppliedNameUpdate> AppliedNameUpdates { get; set; }
        public DbSet<AppliedTransfer> AppliedTransfers { get; set; }
        public DbSet<Appraisal> Appraisals { get; set; }
        public DbSet<ApprovalBoard> ApprovalBoards { get; set; }
        public DbSet<ApprovalWorkItem> ApprovalWorkItems { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<EducationalGrade> EducationalGrades { get; set; }
        public DbSet<EducationalLevel> EducationalLevels { get; set; }
        public DbSet<EducationalQualification> EducationalQualification { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeAddress> EmployeesAddress { get; set; }
        public DbSet<EmployeeApprovalConfig> EmployeesApprovalConfig { get; set; }
        public DbSet<EmployeeEducationDetail> EmployeesEducationDetail { get; set; }
        public DbSet<EmployeeNOKDetail> EmployeeNOKDetails { get; set; }
        public DbSet<GradeLevel> GradeLevels { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeaveRecall> LeaveRecalls { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<Role> RoleType { get; set; }
       // public DbSet<Training>  Trainings { get; set; }
        public DbSet<Transfer>  Transfers { get; set; }
        public DbSet<Unit>  Units { get; set; }
        public DbSet<UserRole> EmployeeRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
