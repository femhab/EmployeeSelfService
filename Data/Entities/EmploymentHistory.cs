using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class EmploymentHistory: BaseObject
    {
        public Guid EmployeeId { get; set; } 
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public Guid JobPostionId { get; set; } //depend on HRPosition
        public JobPostion Position { get; set; }
        public Guid JobChangeReasonId { get; set; }
        public JobChangeReason JobChangeReason { get; set; }
        public Guid? GradeLevelId { get; set; }
        public GradeLevel GradeLevel { get; set; }
        public Guid? DivisionId { get; set; }
        public Division Division { get; set; }
        public Guid? DepartmentId { get; set; }
        public Department Department { get; set; }
        public Guid? SectionId { get; set; }
        public Section Section { get; set; }
        public Guid? UnitId { get; set; }
        public Unit Unit { get; set; }
        public Guid? LocationId { get; set; }
        public Location Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
