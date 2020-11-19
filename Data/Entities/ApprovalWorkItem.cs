using Data.Entities.Common;
using Data.Enums;

namespace Data.Entities
{
    public class ApprovalWorkItem: BaseObject
    {
        public string Name { get; set; } //Leave, Loan
        public string Description { get; set; } //Leave application, Loan application
        public Status Status { get; set; } // //Active, Not Active
    }
}
