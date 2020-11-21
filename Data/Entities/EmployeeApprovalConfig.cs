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
        public Level ApprovalLevel { get; set; }
        public Guid ProcessorIId { get; set; }
    }
}
