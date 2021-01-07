using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class TrainingFeedback: BaseObject
    {
        public Guid TrainingId { get; set; }
        public Training Training { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Feedback { get; set; }
        public string OverallAssessment { get; set; }
    }
}
