using Data.Entities.Common;
using Data.Enums;
using System;

namespace Data.Entities
{
    public class PaymentAdvance: BaseObject
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public DateTime TargetDate { get; set; }
        public decimal Amount { get; set; } //max 100%
        public ApprovalStatus Status { get; set; }
    }
}
