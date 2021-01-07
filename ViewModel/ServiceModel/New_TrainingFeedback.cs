namespace ViewModel.ServiceModel
{
    public class New_TrainingFeedback: BaseServiceModel
    {
        public int New_TrainingFeedbackID { get; set; }
        public string TrainerFeedback { get; set; }
        public string TrainerOverallAssesment { get; set; }
        public string TraineeFeedback { get; set; }
        public string TraineeOverallAssesment { get; set; }
        public int New_TrainingAttendanceID { get; set; }
    }
}
