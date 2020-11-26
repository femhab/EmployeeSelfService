using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class AppraisalPeriod: BaseObject
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
