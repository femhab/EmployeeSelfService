using System;
using System.Collections.Generic;
using ViewModel.Enumeration;

namespace ViewModel.Model
{
    public class ExitProcessModel: BaseModel
    {
        public Guid EmployeeId { get; set; }
        public EmployeeModel Employee { get; set; }
        public string Emp_No { get; set; }
        public ExitProcessStatusEnum Status { get; set; }
        public string Reason { get; set; }
        public DateTime NoticeDate { get; set; }
        public DateTime ExitDate { get; set; }
    }

    public class ExitProcessViewModel
    {
        public EmployeeModel Employee { get; set; }
        public IEnumerable<ExitProcessModel> ExitProcessList { get; set; }
        public IEnumerable<DepartmentModel> ClearanceDepartment { get; set; }
    }
}
