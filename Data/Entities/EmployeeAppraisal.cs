using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class EmployeeAppraisal : BaseObject
    {
        public Guid EmployeeId { get; set; } //EmployeeId
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public Guid AppraisalPeriodId { get; set; }
        public AppraisalPeriod AppraisalPeriod { get; set; }
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
    }

    public class AppraisalItem: BaseObject
    {
        public Guid EmployeeAppraisalId { get; set; }
        public EmployeeAppraisal EmployeeAppraisal { get; set; }
        public Guid AppraisalCategoryId { get; set; }
        public AppraisalCategory AppraisalCategory { get; set; }
        public Guid AppraisalCategoryItemId { get; set; }
        public AppraisalCategoryItem AppraisalCategoryItem { get; set; }
        public Guid AppraisalRatingId { get; set; }
        public AppraisalRating AppraisalRating { get; set; }
    }
}
