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
        public const string NoRecordExist = "No record exist";
        public const string MaximumReached = "Maximum record allowed reached! update/delete from the existing record";
        public const string AlreadyApproved = "This request is already approved with this action";
        public const string ApprovedSuccessfully = "This request is approved successfully";
        public const string QueryCreatedSuccessfully = "You successfully submitted a query";
        public const string LoanCreatedSuccessfully = "You loan request was submitted successfully";
        public const string LoanTypeExist = "You already have an ongoing/approved/pending loan of this type";
        public const string LoanProcessStarted = "You can not update a loan when process already started";
    }

    public class HRDbConfig
    {
        public const string ConnectionStringUrl = "Server=localhost\\SQLEXPRESS;Database=PEERSHR;Integrated Security=true;";
    }

    public class PayrollDbConfig
    {
        public const string ConnectionStringUrl = "Server=localhost\\SQLEXPRESS;Database=PEERSHR;Integrated Security=true;";
    }
}
