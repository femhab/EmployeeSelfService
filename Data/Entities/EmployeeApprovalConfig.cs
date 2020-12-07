using Data.Entities.Common;
using Data.Enums;
using System;

namespace Data.Entities
{
    public class EmployeeApprovalConfig: BaseObject
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public Level ApprovalLevel { get; set; } //first level
        public Guid? ProcessorIId { get; set; }
        public string Processor { get; set; }
        public Guid ApprovalWorkItemId { get; set; }
        public ApprovalWorkItem ApprovalWorkItem { get; set; } //leave service
    }
}
