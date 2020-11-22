namespace ViewModel.ResponseModel
{
    public class ResponseMessage
    {
        public const string CreatedSuccessful = "Record successfully created";
        public const string DeletedSuccessful = "Record successfully deleted";
        public const string UpdatedSuccessful = "Record successfully updated";
        public const string ApprovedSuccessful = "Record successfully approved";
        public const string AwaitingApproval = "Request awaiting approval";
        public const string OperationFailed = "Oop!, Please try again later";
        public const string LeaveExist = "You already enjoyed a leave or on a/pending leave request. If it is a recall case, please proceed to request leave recall instead";
        public const string MaximumLeaveReached = "Maximum allowed leave days reached";
        public const string LeaveExecuted = "No record found/ leave process already executed";
        public const string RecordExist = "Record already exist";
        public const string MaximumReached = "Maximum record allowed reached! update/delete from the existing record";
        public const string AlreadyApproved = "This request is already approved with this action";
    }
}
