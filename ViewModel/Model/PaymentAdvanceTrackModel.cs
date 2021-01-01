using System;

namespace ViewModel.Model
{
    public class PaymentAdvanceTrackModel: BaseModel
    {
        public Guid EmployeeId { get; set; }
        public EmployeeModel Employee { get; set; }
        public string Emp_No { get; set; }
        public int Count { get; set; } //max 100%
    }
}
