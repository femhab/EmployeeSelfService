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
        public const string MaximumPaymentAdvanceApplied = "“You cannot take more than 3 salary advance in a fiscal year”.";
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
        public const string SignOffSuccessful = "The service was signed off successfully for next approval";
        public const string MaxNokReached = "You are only allowed to have 2 Next of Kin";
        public const string MaxEmerReached = "You are only allowed to have 2 Emergency Contact";
    }

    public class NotificationAction
    {
        public const string RoleCreateTitle = "New Role Added";
        public const string FeedbackCreateTitle = "New Feedback";
        public const string DisciplinaryCreateTitle = "New query";
        public const string AppraisalCreateTitle = "New Appraisal";
        public const string TransferCreateTitle = "New Transfer Request";
        public const string AdvanceCreateTitle = "New Payment Advance Request";
        public const string LeaveCreateTitle = "New Leave";
        public const string NOKCreateTitle = "New Next of Kin";
        public const string LoanCreateTitle = "New Loan Requested";
        public const string DependentCreateTitle = "New Dependent";
        public const string TrainingCreateTitle = "New Training";
        public const string RoleCreateMessage = "A new role is just added, confirm from profile if you have been assigned to this role";
        public const string AppraisalCreateMessage = "You just submitted a new appraisal. Please not that you are to signoff after every review ny the approval manager";
        public const string AdvanceCreateMessage = "You just requested for a payment advance... Exercise patient while the managers see to the request approval";
        public const string DisciplinaryCreateMessage = "You just issued a query to a lower grade staff.";
        public const string LeaveCreateMessage = "Your leave request was recieved successfully";
        public const string NOKCreateMessage = "You just added a next of kin. Wait while it is reviewed for approval";
        public const string LoanCreateMessage = "You just requested for a loan... Exercise patient while the managers see to the request approval";
        public const string TrainingCreateMessage = "You just applied to partake in a training";
        public const string TransferCreateMessage = "You just applied for a new transfer";
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
