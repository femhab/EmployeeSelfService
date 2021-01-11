using System;

namespace ViewModel.ServiceModel
{
    public class New_TrainingCalendar : BaseServiceModel
    {
        public int New_TrainingCalendarID { get; set; }
        public int? TrainingYear { get; set; }
        public string Topic { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Organiser { get; set; }
        public string Venue { get; set; }
        public decimal AmtPerHead { get; set; }
        public bool InternalFlag { get; set; }
        public int? New_TrainingRoomID { get; set; }
        public string Username { get; set; }
        public int? HoursPerDay { get; set; }
        public int? IsInternational { get; set; }
    }
}
