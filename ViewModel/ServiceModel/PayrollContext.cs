using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ViewModel.ServiceModel
{
    public class PayrollContext : DbContext
    {
        public PayrollContext(DbContextOptions<PayrollContext> options) : base(options) { }

        public virtual DbSet<BasicReportModel> BasicReportModels { get; set; }
        public virtual DbSet<EarningReportModel> EarningReportModels { get; set; }
        public virtual DbSet<DeductionReportModel> DeductionReportModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BasicReportModel>(f =>
            {
                f.HasKey(e => e.emp_no);
            });
            modelBuilder.Entity<EarningReportModel>(f =>
            {
                f.HasKey(e => e.prz_payTypCode);
            });
            modelBuilder.Entity<DeductionReportModel>(f =>
            {
                f.HasKey(e => e.deductions);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
