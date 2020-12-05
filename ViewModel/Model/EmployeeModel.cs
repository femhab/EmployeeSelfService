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
        public Guid UnitId { get; set; }
        public UnitModel Unit { get; set; }
        public AccessTypeEnum AccessType { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime? DOB { get; set; }
        public GenderEnum Gender { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public DateTime? EmploymentDate { get; set; }
        public string PensionNo { get; set; }
    }

    public class HRUserModel
    {
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string EmailAddress { get; set; }
        public string Emp_No { get; set; }
    }

    public class EmployeeViewModel
    {
        public IEnumerable<EmployeeModel> EmployeeList { get; set; }
        public IEnumerable<HRUserModel> Employeee { get; set; }
        public IEnumerable<RoleModel> Roles { get; set; }
        public IEnumerable<DepartmentModel> Departments { get; set; }
    }

    public class EmployeeProfileViewModel
    {
        public EmployeeModel Employee { get; set; }
        public int ApprovalCount { get; set; }
        public IEnumerable<ApprovalWorkItemModel> ApprovalWorkItem { get; set; }
        public IEnumerable<RelationshipModel> Relationshiop { get; set; }
        public IEnumerable<EmployeeFamilyDependentModel> Dependents { get; set; }
        public IEnumerable<EmployeeNOKDetailModel> NOKDetails { get; set; }
    }
}
