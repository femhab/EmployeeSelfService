using Data.Entities.Common;
using Data.Enums;
using System;

namespace Data.Entities
{
    public class ApprovalBoardActiveLevel: BaseObject
    {
        public Guid ApprovalWorkItemId { get; set; } //WorkItemId(Leave, Loan)
        public ApprovalWorkItem ApprovalWorkItem { get; set; }
        public Guid ServiceId { get; set; } //leaveId, LoadId
        public Level ActiveLevel { get; set; }
    }
}
