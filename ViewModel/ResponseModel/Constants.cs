namespace ViewModel.ResponseModel
{
    public class ResponseMessage
    {
        public const string OperationSuccessful = "Record retrieved successfully";
        public const string CreatedSuccessful = "Record successfully created";
        public const string DeletedSuccessful = "Record successfully deleted";
        public const string UpdatedSuccessful = "Record successfully updated";
        public const string ApprovedSuccessful = "Record successfully approved";
        public const string AwaitingApproval = "Request awaiting approval";
        public const string OperationFailed = "Oop!, Please try again later";
        public const string LeaveExist = "You already enjoyed a leave or on a/pending leave request. If it is a recall case, please proceed to request leave recall instead";
        public const string MaximumLeaveReached = "Maximum allowed leave days reached";
        public const string MaximumPaymentAdvanceApplied = "Maximum application in a year is reahed";
        public const string PaymentAdvanceExist = "You already requested an advance for the month";
        public const string LeaveExecuted = "No record found/ leave process already executed";
        public const string LeaveRecallExecuted = "No record found/ leave recall already requested";
        public const string RecordExist = "Record already exist";
        public const string NoRecordExist = "No record exist";
        public const string MaximumReached = "Maximum record allowed reached! update/delete from the existing record";
        public const string AlreadyApproved = "This request is already approved with this action";
        public const string ApprovedSuccessfully = "This request is approved successfully";
        public const string QueryCreatedSuccessfully = "You successfully submitted a query";
        public const string LoanCreatedSuccessfully = "You loan request was submitted successfully";
        public const string LoanTypeExist = "You already have an ongoing/approved/pending loan of this type";
        public const string LoanProcessStarted = "You can not update a loan when process already started";
        public const string AppraisalExist = "You already submitted an appraisal for this year";
    }

    public class NotificationAction
    {
        public const string RoleCreateTitle = "New Role Added";
        public const string FeedbackCreateTitle = "New Feedback";
        public const string DisciplinaryCreateTitle = "New query";
        public const string LeaveCreateTitle = "New Leave";
        public const string NOKCreateTitle = "New Next of Kin";
        public const string DependentCreateTitle = "New Dependent";
        public const string TrainingCreateTitle = "New Training";
        public const string RoleCreateMessage = "A new role is just added, confirm from profile if you have been assigned to this role";
        public const string DisciplinaryCreateMessage = "You just issued a query to a lower grade staff.";
        public const string LeaveCreateMessage = "Your leave request was recieved successfully";
        public const string NOKCreateMessage = "You just added a next of kin. Wait while it is reviewed for approval";
        public const string TrainingCreateMessage = "You just applied to partake in a training";
        public const string DependentCreateMessage = "You just added a dependent. Wait while it is reviewed for approval";
        public const string RoleDeletedTitle = "A Role Deleted";
        public const string RoleDeleteMessage = "A role is just deleted, if you have this role before, you no longer have this role";
    }

    public class HRDbConfig
    {
        public const string ConnectionStringUrl = "Server=localhost\\SQLEXPRESS;Database=PEERSHR;Integrated Security=true;";
    }

    public class PayrollDbConfig
    {
        public const string ConnectionStringUrl = "Server=localhost\\SQLEXPRESS;Database=PEERSPR;Integrated Security=true;";
    }
}
