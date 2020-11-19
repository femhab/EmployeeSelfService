using Data.Entities.Common;
using Data.Enums;
using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class ApprovalBoard: BaseObject
    {
        public Guid EmployeeId { get; set; } //EmployeeId
        public Employee Employee { get; set; } 
        public string Emp_No { get; set; }
        public Guid ApprovalWorkItemId { get; set; } //WorkItemId(Leave, Loan)
        public List<ProcessedWorkItem> ProcessedWorkItem { get; set; }
    }

    public class ProcessedWorkItem
    {
        public Guid ApprovalSettingId { get; set; } //Approval setting i.e First Level
        public ApprovalSetting ApprovalSetting { get; set; }  //
        public string ApprovalProcessor { get; set; } // processor employee no
        public ApprovalStatus Status { get; set; } //new, pending
    }
}
