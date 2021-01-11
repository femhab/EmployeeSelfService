using System;

namespace ViewModel.Model
{
    public class TrainingNominationModel: BaseModel
    {
        public Guid EmployeeId { get; set; }
        public EmployeeModel Employee { get; set; }
        public Guid TrainingCalenderId { get; set; }
        public TrainingCalenderModel TrainingCalender { get; set; }
        public string Emp_No { get; set; }
        public int HRTrainingNominationID { get; set; }
        public int HRTrainingCalendarID { get; set; }
        public bool IsApplied { get; set; }
    }
}
