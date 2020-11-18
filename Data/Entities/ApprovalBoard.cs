using Data.Entities.Common;
using Data.Enums;
using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class ApprovalBoard: BaseObject
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public Guid ApprovalWorkItemId { get; set; }
        public List<ApprovalWorkItem> ApprovalWorkItem { get; set; }
    }

    public class ProcessedWorkItem
    {
        public Guid ApprovalSettingId { get; set; }
        public ApprovalSetting ApprovalSetting { get; set; }
        public string ApprovalProcessor { get; set; }
        public ApprovalStatus Status { get; set; }
    }
}
