using Data.Entities.Common;
using Data.Enums;
using System;

namespace Data.Entities
{
    public class LeaveType: BaseObject
    {
        public LeaveClass Class { get; set; }
        public Guid GradeLevelId { get; set; }
        public GradeLevel GradeLevel { get; set; }
        public int AvailableDays { get; set; }
    }
}
