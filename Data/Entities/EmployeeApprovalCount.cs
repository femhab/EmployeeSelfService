using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class EmployeeApprovalCount: BaseObject
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public Guid ApprovalWorkItemId { get; set; }
        public ApprovalWorkItem ApprovalWorkItem { get; set; } //leave service
        public int MaximumCount { get; set; }
    }
}
