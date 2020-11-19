using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class EmployeeAddress: BaseObject
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public string StreetAddress { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string StateOfOrigin { get; set; }
        public string LGOfOrigin { get; set; }
    }
}
