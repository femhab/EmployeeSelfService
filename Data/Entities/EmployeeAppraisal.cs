using Data.Entities.Common;
using Data.Enums;
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
