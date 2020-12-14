using System;

namespace ViewModel.Model
{
    public class EmployeeAppraisalModel: BaseModel
    {
        public Guid EmployeeId { get; set; } //EmployeeId
        public EmployeeModel Employee { get; set; }
        public string Emp_No { get; set; }
        public Guid AppraisalPeriodId { get; set; }
        public AppraisalPeriodModel AppraisalPeriod { get; set; }
        public string LastRatingManagerId { get; set; }
        public string LastRatingManagerName { get; set; }
        public string NextRatingManagerId { get; set; }
        public string NextRatingManagerName { get; set; }
    }

    public class AppraisalItemModel : BaseModel
    {
        public Guid EmployeeAppraisalId { get; set; }
        public EmployeeAppraisalModel EmployeeAppraisal { get; set; }
        public Guid AppraisalCategoryId { get; set; }
        public AppraisalCategoryModel AppraisalCategory { get; set; }
        public Guid AppraisalCategoryItemId { get; set; }
        public AppraisalCategoryItemModel AppraisalCategoryItem { get; set; }
        public Guid AppraisalRatingId { get; set; }
        public AppraisalRatingModel AppraisalRating { get; set; }
    }
}
