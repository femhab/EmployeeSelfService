using System;
using ViewModel.Enumeration;

namespace ViewModel.Model
{
    public class LeaveTypeModel: BaseModel
    {
        public LeaveClassEnum Class { get; set; }
        public Guid GradeLevelId { get; set; }
        public GradeLevelModel GradeLevel { get; set; }
        public int AvailableDays { get; set; }
    }
}
