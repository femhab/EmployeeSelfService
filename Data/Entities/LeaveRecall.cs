using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class LeaveRecall: BaseObject
    {
        public Guid LeaveId { get; set; }
        public Leave Leave { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
