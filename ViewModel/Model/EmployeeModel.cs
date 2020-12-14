using System;
using System.Collections.Generic;
using ViewModel.Enumeration;

namespace ViewModel.Model
{
    public class EmployeeModel: BaseModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EmailAddress { get; set; }
        public string Emp_No { get; set; }
        public string StaffType { get; set; }
        public Guid GradeLevelId { get; set; }
        public GradeLevelModel GradeLevel { get; set; }
        public Guid DivisionId { get; set; }
        public DivisionModel Division { get; set; }
        public Guid DepartmentId { get; set; }
        public DepartmentModel Department { get; set; }
        public Guid? SectionId { get; set; }
        public SectionModel Section { get; set; }
        public Guid? UnitId { get; set; }
        public UnitModel Unit { get; set; }
        public Guid? LocationId { get; set; }
        public LocationModel Location { get; set; }
        public Guid? CourtesyId { get; set; }
        public CourtesyModel Courtesy { get; set; }
        public Guid? EmployeeTitleId { get; set; }
        public EmployeeTitleModel EmployeeTitle { get; set; }
        public Guid? CountryId { get; set; }
        public CountryModel Country { get; set; }
        public Guid? StateId { get; set; }
        public StateModel State { get; set; }
        public Guid? LGAId { get; set; }
        public LGAModel LGA { get; set; }
        public AccessTypeEnum AccessType { get; set; }
        public StatusEnum Status { get; set; }
        public Guid? AvalaibilityStatusId { get; set; }
        public AvalaibilityStatusModel AvalaibilityStatus { get; set; }
        public Guid? MaritalStatusId { get; set; }
        public MaritalStatusModel MaritalStatus { get; set; }
        public DateTime? DOB { get; set; }
        public GenderEnum Gender { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public DateTime? EmploymentDate { get; set; }
        public string PensionNo { get; set; }
        public DateTime? DateConf { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? PreAppDate { get; set; }
        public DateTime? ProRetireDate { get; set; }
    }

    public class HRUserModel
    {
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EmailAddress { get; set; }
        public string Emp_No { get; set; }
    }

    public class EmployeeViewModel: AuthDataModel
    {
        public IEnumerable<EmployeeModel> EmployeeList { get; set; }
        public IEnumerable<HRUserModel> Employeee { get; set; }
        public IEnumerable<RoleModel> Roles { get; set; }
        public IEnumerable<DepartmentModel> Departments { get; set; }
    }

    public class EmployeeProfileViewModel: AuthDataModel
    {
        public EmployeeModel Employee { get; set; }
        public int ApprovalCount { get; set; }
        public IEnumerable<ApprovalWorkItemModel> ApprovalWorkItem { get; set; }
        public IEnumerable<RelationshipModel> Relationshiop { get; set; }
        public IEnumerable<EmployeeFamilyDependentModel> Dependents { get; set; }
        public IEnumerable<EmployeeNOKDetailModel> NOKDetails { get; set; }
        public IEnumerable<UserRoleModel> UserRoles { get; set; }
        public IEnumerable<ApprovalWorkItemModel> ApprovalWorkItems { get; set; }
        public IEnumerable<EducationalGradeModel> EducationalGrade { get; set; }
        public IEnumerable<EducationalLevelModel> EducationalLevel { get; set; }
        public IEnumerable<EducationalQualificationModel> EducationalQualification { get; set; }
        public IEnumerable<EmployeeEducationDetailModel> EmployeeEducationDetail { get; set; }
        public IEnumerable<EmployeeApprovalconfigModel> EmployeeApprovalconfig { get; set; }
        public IEnumerable<DivisionModel> Division { get; set; }
        public IEnumerable<DepartmentModel> Department { get; set; }
        public IEnumerable<UnitModel> Unit { get; set; }
        public IEnumerable<SectionModel> Section { get; set; }
    }
}
