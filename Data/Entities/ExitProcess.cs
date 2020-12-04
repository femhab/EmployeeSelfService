using Data.Entities.Common;
using Data.Enums;
using System;

namespace Data.Entities
{
    public class ExitProcess : BaseObject 
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public ExitProcessStatus Status { get; set; }
    }
}
