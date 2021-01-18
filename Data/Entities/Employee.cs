using Data.Entities.Common;
using Data.Enums;
using System;

namespace Data.Entities
{
    public class Employee : BaseObject
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EmailAddress { get; set; }
        public string Emp_No { get; set; }
        public Guid? GradeLevelId { get; set; }
        public GradeLevel GradeLevel { get; set; }
        public Guid? DivisionId { get; set; }
        public Division Division { get; set; }
        public Guid? DepartmentId { get; set; }
        public Department Department { get; set; }
        public Guid? SectionId { get; set; }
        public Section Section { get; set; }
        public Guid? UnitId { get; set; }
        public Unit Unit { get; set; }
        public Guid? LocationId { get; set; }
        public Location Location { get; set; }
        public Guid? CourtesyId { get; set; }
        public Courtesy Courtesy { get; set; }
        public Guid? EmployeeTitleId { get; set; }
        public EmployeeTitle EmployeeTitle { get; set; }
        public Guid? CountryId { get; set; }
        public Country Country { get; set; }
        public Guid? StateId { get; set; }
        public State State { get; set; }
        public Guid? LGAId { get; set; }
        public LGA LGA { get; set; }
        public string StaffType { get; set; }
        public AccessType AccessType { get; set; }
        public Status Status { get; set; }
        public Guid? AvalaibilityStatusId { get; set; }
        public AvalaibilityStatus AvalaibilityStatus { get; set; }
        public Guid? MaritalStatusId { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public DateTime? DOB { get; set; }
        public Gender? Gender { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public DateTime? EmploymentDate { get; set; }
        public string PensionNo { get; set; }
        public DateTime? DateConf { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? PreAppDate { get; set; }
        public DateTime? ProRetireDate { get; set; }
        public string ReportToLineManager { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class AppliedNameUpdate : BaseObject
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EmailAddress { get; set; }
        public string Emp_No { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }

    public class AppliedTransfer : BaseObject
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public Guid DivisionId { get; set; }
        public Division Division { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        public Guid SectionId { get; set; }
        public Section Section { get; set; }
        public Guid? UnitId { get; set; }
        public Unit Unit { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
