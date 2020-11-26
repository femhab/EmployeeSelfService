namespace ViewModel.ServiceModel
{
    public class Appraisal_EmpRating
    {
        public int Appraisal_EmpRatingID { get; set; }
        public int AppraisalRatingID { get; set; }
        public decimal RatingSelf { get; set; }
        public decimal RatingManager { get; set; }
        public int AppraisalPeriodID { get; set; }
        public string Emp_No { get; set; }
    }
}
