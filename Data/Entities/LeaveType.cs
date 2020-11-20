using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class LeaveType: BaseObject
    {
        public string Name { get; set; }
        public Guid GradeLevelId { get; set; }
        public GradeLevel GradeLevel { get; set; }
        public int AvailableDays { get; set; }
    }
}
