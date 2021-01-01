﻿using Data.Entities.Common;

namespace Data.Entities
{
    public class TrainingCalender: BaseObject
    {
        //public int TrainingCalendarID { get; set; }
        public int TrainingYear { get; set; }
        public string Topic { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Organiser { get; set; }
        public string Venue { get; set; }
        public decimal AmtPerHead { get; set; }
        public bool InternalFlag { get; set; }
        public int TrainingRoomID { get; set; }
        public string Username { get; set; }
        public int HoursPerDay { get; set; }
        public int IsInternational { get; set; }
        public string Emp_no { get; set; }
        public string TrainingCategory { get; set; }
        public bool Attended { get; set; }
    }
}