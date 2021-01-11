using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class TrainingNomination: BaseObject
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public Guid TrainingCalenderId { get; set; }
        public TrainingCalender TrainingCalender { get; set; }
        public string Emp_No { get; set; }
        public int HRTrainingNominationID { get; set; }
        public int HRTrainingCalendarID { get; set; }
        public bool IsApplied { get; set; }
    }
}
