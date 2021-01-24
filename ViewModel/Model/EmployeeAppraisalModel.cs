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
        public string Strenght { get; set; }
        public string Weekness { get; set; }
        public string Counselling { get; set; }
        public string Redeployment { get; set; }
        public string Development { get; set; }
        public string DisciplinaryAction { get; set; }
        public string Training { get; set; }
        public string Promotion { get; set; }
        public string OtherDetail { get; set; }
        public decimal TotalScore { get; set; }
        public decimal TotalNetScore { get; set; }
        public string AppraiseeComment { get; set; }
        public string ManagerComment { get; set; }
        public string AreaOfImprovement { get; set; }
        public string AppraisalTarget { get; set; }
        public bool IsEmployeeSignOff { get; set; }
        public bool IsManagerSignOff { get; set; }
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

    public class AppraisalItemUpdateModel
    {
        public Guid EmployeeAppraisalId { get; set; }
        public Guid CategoryItemId { get; set; }
        public Guid RatingId { get; set; }
    }
}
