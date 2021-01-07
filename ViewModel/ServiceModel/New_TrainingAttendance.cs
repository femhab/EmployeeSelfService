namespace ViewModel.ServiceModel
{
    public class New_TrainingAttendance: BaseServiceModel
    {
        public int New_TrainingAttendanceID { get; set; }
        public int New_TrainingNominationID { get; set; }
        public string TrainerFeedback { get; set; }
        public string TrainerOverallAssessment { get; set; }
        public string TraineeFeedback { get; set; }
        public string TraineeOverallAssessment { get; set; }
        public decimal OtherCosts { get; set; }
        public string DeptCode { get; set; }
        public decimal PerHeadCost { get; set; }
    }
}
