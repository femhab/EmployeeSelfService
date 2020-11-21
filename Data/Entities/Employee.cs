using Data.Entities.Common;
using Data.Enums;
using System;
using System.Collections.Generic;

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
        public Guid GradeLevelId { get; set; }
        public GradeLevel GradeLevel { get; set; }
        public Guid DivisionId { get; set; }
        public Division Division { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        public Guid UnitId { get; set; }
        public Unit Unit { get; set; }
        public AccessType AccessType { get; set; }
        public Status Status { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public DateTime? DOB { get; set; }
        public Gender Gender { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public DateTime? EmploymentDate { get; set; }
        public string PensionNo { get; set; }
        public int? MaximumApprovalCount { get; set; }
    }
}
