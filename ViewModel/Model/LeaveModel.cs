using System;
using System.Collections.Generic;
using ViewModel.Enumeration;

namespace ViewModel.Model
{
    public class LeaveModel: BaseModel
    {
        public Guid EmployeeId { get; set; } //employeeId 
        public EmployeeModel Employee { get; set; }
        public string Emp_No { get; set; }
        public Guid LeaveTypeId { get; set; } //annual leave, paternity leave
        public LeaveTypeModel LeaveType { get; set; } //leavetype per gradelevel
        public DateTime DateFrom { get; set; } //to be specified
        public DateTime DateTo { get; set; } //to be specified
        public DateTime? ActualEndDate { get; set; } //to be specified
        public bool IsFiveYearsAnniversary { get; set; }
        public bool IsTenYearsAnniversary { get; set; }
        public DateTime? ResumptionDate { get; set; }
        public int NoOfDays { get; set; }
        public int? DaysUsed { get; set; }
        public ApprovalStatusEnum Status { get; set; } //new, pending
        public LeaveStatusEnum LeaveStatus { get; set; }
        public bool IsAllowanceRequested { get; set; }
        public string LastProccessedBy { get; set; }
    }

    public class LeaveViewModel
    {
        public EmployeeModel Employee { get; set; }
        public IEnumerable<LeaveTypeModel> LeaveType { get; set; }
        public IEnumerable<LeaveModel> LeaveTaken { get; set; }
    }
}
