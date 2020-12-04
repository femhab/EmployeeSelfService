using Data.Entities.Common;
using Data.Enums;
using System;

namespace Data.Entities
{
    public class DisciplinaryAction : BaseObject
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public string QuerySubject { get; set; }
        public string QueryMessage { get; set; }
        public DateTime QueryDate { get; set; }
        public string QueryReply { get; set; }
        public DateTime? QueryReplyDate { get; set; }
        public string QueryActionComment { get; set; }
        public Guid InitiatorId { get; set; }
        public DateTime? QueryActionDate { get; set; }
        public QueryAction Action { get; set; }
    }
}
