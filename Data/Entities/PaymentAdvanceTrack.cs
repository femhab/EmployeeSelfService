using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class PaymentAdvanceTrack: BaseObject
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public int Count { get; set; }
    }
}
