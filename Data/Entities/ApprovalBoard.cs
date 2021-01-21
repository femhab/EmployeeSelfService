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
        public ApprovalWorkItem ApprovalWorkItem { get; set; }
        public Guid ServiceId { get; set; } //leaveId, LoadId
        public Level ApprovalLevel { get; set; } //Approval setting i.e First Level
        public Guid ApprovalProcessorId { get; set; } // processor employee no
        public string ApprovalProcessor { get; set; } // processor employee no
        public ApprovalStatus Status { get; set; } //new, pending
        public bool SignOff { get; set; }//applicable to only appraisal
        public bool EmployeeReview { get; set; }
        public bool ManagerSignOff { get; set; }
    }
}
