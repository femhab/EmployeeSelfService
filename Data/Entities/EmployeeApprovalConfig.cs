using Data.Entities.Common;
using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class EmployeeApprovalConfig: BaseObject
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public List<ApprovalLevel> ApprovalProcessor { get; set; }
    }

    public class ApprovalLevel
    {
        public Guid ApprovalSettingId { get; set; }
        public ApprovalSetting ApprovalSetting { get; set; }
        public string Emp_No { get; set; }
    }
}
