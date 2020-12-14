using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class LeaveRecall: BaseObject
    {
        public Guid LeaveId { get; set; }
        public Leave Leave { get; set; }
        public DateTime RecallDate { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime ResumptionDate { get; set; }
        public int NoOfDays { get; set; }
    }
}
