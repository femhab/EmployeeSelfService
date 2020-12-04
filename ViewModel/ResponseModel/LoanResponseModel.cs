namespace ViewModel.ResponseModel
{
    public class LoanResponseModel
    {
    }
    public class LoanEligibilityResponseModel: BaseResponse
    {
        public decimal OutstandingAmount { get; set; } 
        public decimal Loanlimit { get; set; }
    }

}
