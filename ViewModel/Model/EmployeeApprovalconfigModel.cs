using System;
using System.Collections.Generic;
using System.Text;
using ViewModel.Enumeration;

namespace ViewModel.Model
{
    public class EmployeeApprovalconfigModel: BaseModel
    {
        public Guid EmployeeId { get; set; }
        public EmployeeModel Employee { get; set; }
        public string Emp_No { get; set; }
        public LevelEnum ApprovalLevel { get; set; } //first level
        public Guid? ProcessorIId { get; set; }
        public string Processor { get; set; }
        public Guid ApprovalWorkItemId { get; set; }
        public ApprovalWorkItemModel ApprovalWorkItem { get; set; } //leave service
    }
}
