using Data.Entities.Common;
using Data.Enums;
using System;

namespace Data.Entities
{
    public class Leave: BaseObject
    {
        public Guid EmployeeId { get; set; } //employeeId 
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public Guid LeaveTypeId { get; set; } //annual leave, paternity leave
        public LeaveType LeaveType { get; set; } //leavetype per gradelevel
        public DateTime DateFrom { get; set; } //to be specified
        public DateTime DateTo { get; set; } //to be specified
        public bool IsFiveYearsAnniversary { get; set; }
        public bool IsTenYearsAnniversary { get; set; }
        public DateTime? ResumptionDate { get; set; }
        public int NoOfDays { get; set; }
        public int? DaysUsed { get; set; }
        public ApprovalStatus Status { get; set; } //new, pending
        public LeaveStatus LeaveStatus { get; set; }
        public bool IsAllowanceRequested { get; set; }
        public string LastProccessedBy { get; set; }
    }
}
