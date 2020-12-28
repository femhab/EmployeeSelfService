namespace ViewModel.ResponseModel
{
    public class DashboardResponseModel: BaseResponse
    {
        public int ApprovalPending { get; set; }
        public int ApprovalDone { get; set; }
        public int NewUserCount { get; set; }
        public int LeaveApprovedInDepartment { get; set; }
        public int LeaveOngoingInDepartment { get; set; }
        public int LeaveDaysEligible { get; set; }
        public int AnnualLeaveDaysLimit { get; set; }
        public decimal LoanAmountEligible { get; set; }
        public decimal LoanAmountLimit { get; set; }
    }
}
