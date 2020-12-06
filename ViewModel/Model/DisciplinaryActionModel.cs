using System;
using System.Collections.Generic;
using ViewModel.Enumeration;

namespace ViewModel.Model
{
    public class DisciplinaryActionModel: BaseModel
    {
        public Guid EmployeeId { get; set; }
        public EmployeeModel Employee { get; set; }
        public string Emp_No { get; set; }
        public string QuerySubject { get; set; }
        public string QueryMessage { get; set; }
        public DateTime QueryDate { get; set; }
        public string QueryReply { get; set; }
        public DateTime? QueryReplyDate { get; set; }
        public string QueryActionComment { get; set; }
        public Guid TargetEmployeeId { get; set; }
        public string TargetEmployeeNo { get; set; }
        public DateTime? QueryActionDate { get; set; }
        public QueryActionEnum Action { get; set; }
    }

    public class DisciplinaryActionViewModel
    {
        public IEnumerable<EmployeeModel> Employee { get; set; }
        public IEnumerable<DisciplinaryActionModel> DisciplinaryActions { get; set; }
    }
}
