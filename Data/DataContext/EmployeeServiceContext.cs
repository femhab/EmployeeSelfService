﻿using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.DataContext
{
    public class EmployeeServiceContext: IdentityDbContext<AppIdentityUser>
    {
        public EmployeeServiceContext(DbContextOptions<EmployeeServiceContext> options) : base(options) { }

        public DbSet<AppliedNameUpdate> AppliedNameUpdates { get; set; }
        public DbSet<AppraisalCategory> AppraisalCategories { get; set; }
        public DbSet<AppraisalCategoryItem> AppraisalCategoryItems { get; set; }
        public DbSet<AppraisalItem> AppraisalItems { get; set; }
        public DbSet<AppraisalPeriod> AppraisalPeriods { get; set; }
        public DbSet<AppraisalRating> AppraisalRatings { get; set; }
        public DbSet<AppliedTransfer> AppliedTransfers { get; set; }
        public DbSet<ApprovalBoard> ApprovalBoards { get; set; }
        public DbSet<ApprovalBoardActiveLevel> ApprovalBoardActiveLevels { get; set; }
        public DbSet<ApprovalWorkItem> ApprovalWorkItems { get; set; }
        public DbSet<AvalaibilityStatus> AvalaibilityStatus { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Courtesy> Courtesy { get; set; }
        public DbSet<ContractObjective> ContractObjectives { get; set; }
        public DbSet<ContractItem> ContractItems { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DisciplinaryAction> DisciplinaryActions { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<EducationalGrade> EducationalGrades { get; set; }
        public DbSet<EducationalLevel> EducationalLevels { get; set; }
        public DbSet<EducationalQualification> EducationalQualifications { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeAddress> EmployeesAddress { get; set; }
        public DbSet<EmployeeAppraisal> EmployeeAppraisals { get; set; }
        public DbSet<EmployeeApprovalConfig> EmployeesApprovalConfig { get; set; }
        public DbSet<EmployeeApprovalCount> EmployeeApprovalCounts { get; set; }
        public DbSet<EmployeeEducationDetail> EmployeesEducationDetail { get; set; }
        public DbSet<EmployeeFamilyDependent> EmployeeFamilyDependents { get; set; }
        public DbSet<EmployeeLeave> EmployeeLeaves { get; set; }
        public DbSet<EmployeeNOKDetail> EmployeeNOKDetails { get; set; }
        public DbSet<EmployeeTitle> EmployeeTitles { get; set; }
        public DbSet<EmploymentHistory> EmploymentHistories { get; set; }
        public DbSet<ExitProcess> ExitProcess { get; set; }
        public DbSet<ExitProcessPriorityItem> ExitProcessPriorityItem { get; set; }
        public DbSet<GradeLevel> GradeLevels { get; set; }
        public DbSet<JobPostion> JobPostions { get; set; }
        public DbSet<JobChangeReason> JobChangeReasons { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeaveRecall> LeaveRecalls { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LGA> LGAs { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<MaritalStatus> MaritalStatus { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PaymentAdvance> PaymentAdvances { get; set; }
        public DbSet<PIP> PIP { get; set; }
        public DbSet<PIPItem> PIPItems { get; set; }
        public DbSet<PaymentAdvanceTrack> PaymentAdvanceTracks { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<Role> RoleType { get; set; }
        public DbSet<Section>  Sections { get; set; }
        public DbSet<State>  States { get; set; }
        public DbSet<Transfer>  Transfers { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<TrainingTopics> TrainingTopics { get; set; }
        public DbSet<TrainingCalender> TrainingCalender { get; set; }
        public DbSet<TrainingNomination> TrainingNomination { get; set; }
        public DbSet<Unit>  Units { get; set; }
        public DbSet<UserRole> EmployeeRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Seed();
        }
    }
}
