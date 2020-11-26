using Microsoft.EntityFrameworkCore;

namespace ViewModel.ServiceModel
{
    public class ServiceContext : DbContext
    {
        public ServiceContext(DbContextOptions<ServiceContext> options): base(options) { }

        public DbSet<Appraisal_EmpRating> Appraisal_EmpRating { get; set; }
        public DbSet<AppraisalCategorries> AppraisalCategory { get; set; }
        public DbSet<AppraisalCategoryItems> AppraisalCategoryItem { get; set; }
        public DbSet<AppraisalPeriod> AppraisalPeriod { get; set; }
        public DbSet<AppraisalRatings> AppraisalRating { get; set; }
        public DbSet<HRDepCode> HRDepCode { get; set; }
        public DbSet<HRDept> HRDept { get; set; }
        public DbSet<HRDivision> HRDivision { get; set; }
        public DbSet<HREducGrade> HREducGrade { get; set; }
        public DbSet<HREducLevel> HREducLevel { get; set; }
        public DbSet<HREducQual> HREducQual { get; set; }
        public DbSet<HREmpMst> HREmpMst { get; set; }
        public DbSet<HRGrade> HRGrade { get; set; }
        public DbSet<hrleavedays> hrleavedays { get; set; }
        public DbSet<HRUnit> HRUnit { get; set; }
        public DbSet<HRUsers> HRUsers { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HRUsers>(f => { f.HasKey(e => e.UserID); });
            modelBuilder.Entity<HRDepCode>(f => { f.HasKey(e => e.DepCode); });
            modelBuilder.Entity<HRDept>(f => { f.HasKey(e => e.DeptCode); });
            modelBuilder.Entity<HRDivision>(f => { f.HasKey(e => e.DivisionCode); });
            modelBuilder.Entity<HREducGrade>(f => { f.HasKey(e => e.Gradecode); });
            modelBuilder.Entity<HREducLevel>(f => { f.HasKey(e => e.EducLevelCode); });
            modelBuilder.Entity<HREducQual>(f => { f.HasKey(e => e.qualcode); });
            modelBuilder.Entity<HREmpMst>(f => { f.HasKey(e => e.Emp_No); });
            modelBuilder.Entity<HRGrade>(f => { f.HasKey(e => e.GradeCode); });
            modelBuilder.Entity<HRUnit>(f => { f.HasKey(e => e.UnitCode); });
            modelBuilder.Entity<hrleavedays>(f => { f.HasKey(e => e.gradeCode); });

            base.OnModelCreating(modelBuilder);
        }
    }
}
