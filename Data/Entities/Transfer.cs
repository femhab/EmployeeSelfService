using Data.Entities.Common;
using Data.Enums;
using System;

namespace Data.Entities
{
    public class Transfer: BaseObject
    {
        public Guid EmployeeId { get; set; } //employeeId 
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public Guid ExitingDivisionId { get; set; }
        public Division Division { get; set; }
        public Guid ExitingDepartmentId { get; set; }
        public Department ExitingDepartment { get; set; }
        public Guid ExitingUnitId { get; set; }
        public Unit ExitingUnit { get; set; }
        public Guid ProposedDivisionId { get; set; }
        public Division ProposedDivision { get; set; }
        public Guid ProposedDepartmentId { get; set; }
        public Department ProposedDepartment { get; set; }
        public Guid ProposedUnitId { get; set; }
        public Unit ProposedUnit { get; set; }
        public ApprovalStatus Status { get; set; }
        public DateTime? DateApproved { get; set; }
    }
}
