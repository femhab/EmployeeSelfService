using System;

namespace ViewModel.Model
{
    public class TrainingCalenderModel: BaseModel
    {
        public int HRTrainingCalenderID { get; set; }
        public int? TrainingYear { get; set; }
        public Guid TopicId { get; set; }
        public TrainingTopicModel Topic { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Organiser { get; set; }
        public string Venue { get; set; }
        public decimal AmtPerHead { get; set; }
        public bool InternalFlag { get; set; }
        public int? TrainingRoomID { get; set; }
        public string Username { get; set; }
        public int? HoursPerDay { get; set; }
        public int? IsInternational { get; set; }
    }
}
