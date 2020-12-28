using System;
using System.Collections.Generic;
using ViewModel.Enumeration;

namespace ViewModel.Model
{
    public class ApprovalBaordModel: BaseModel
    {
        public Guid EmployeeId { get; set; } //EmployeeId
        public EmployeeModel Employee { get; set; }
        public string Emp_No { get; set; }
        public Guid ApprovalWorkItemId { get; set; } //WorkItemId(Leave, Loan)
        public ApprovalWorkItemModel ApprovalWorkItem { get; set; }
        public Guid ServiceId { get; set; } //leaveId, LoadId
        public LevelEnum ApprovalLevel { get; set; } //Approval setting i.e First Level
        public Guid ApprovalProcessorId { get; set; } // processor employee no
        public string ApprovalProcessor { get; set; } // processor employee no
        public ApprovalStatusEnum Status { get; set; } //new, pending
    }

    public class ApprovalBoardViewModel: AuthDataModel
    {
        public IEnumerable<ApprovalBaordModel> ApprovalBoard { get; set; }
        public IEnumerable<AppraisalCategoryModel> AppraisalCategories { get; set; }
        public IEnumerable<AppraisalRatingModel> AppraisalRatings { get; set; }
    }
}
