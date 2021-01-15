using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class Training: BaseObject
    {
        public Guid EmployeeId { get; set; } 
        public Employee Employee { get; set; } 
        public string Emp_No { get; set; }
        public string TrainingTopic { get; set; }
        public int TrainingYear { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsScheduled { get; set; }
        public string OtherDetails { get; set; }
        public string Venue { get; set; }
        public string Organizer { get; set; }
        public int? HoursPerDay { get; set; }
        public decimal AmtPerHead { get; set; }
    }
}
