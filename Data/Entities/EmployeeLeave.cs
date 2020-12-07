using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class EmployeeLeave: BaseObject
    {
        public Guid EmployeeId { get; set; } //employeeId 
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public Guid LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public int NoOfEligibleDays { get; set; }
        public int AnnivessaryLeaveBonus { get; set; }
        public int NoOfDaysUsed { get; set; }
    }
}
