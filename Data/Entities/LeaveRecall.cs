using Data.Entities.Common;
using Data.Enums;
using System;

namespace Data.Entities
{
    public class LeaveRecall: BaseObject
    {
        public Guid LeaveId { get; set; }
        public Leave Leave { get; set; }
        public int NoOfDays { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
