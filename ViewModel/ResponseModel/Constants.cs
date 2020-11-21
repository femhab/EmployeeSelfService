namespace ViewModel.ResponseModel
{
    public class ResponseMessage
    {
        public const string CreatedSuccessful = "Record successfully created";
        public const string DeletedSuccessful = "Record successfully deleted";
        public const string UpdatedSuccessful = "Record successfully updated";
        public const string ApprovedSuccessful = "Record successfully approved";
        public const string OperationFailed = "Oop!, Please try again later";
        public const string RecordExist = "Record already exist";
        public const string MaximumReached = "Maximum record allowed reached! update/delete from the existing record";
        public const string AlreadyApproved = "This request is already approved with this action";
    }
}
