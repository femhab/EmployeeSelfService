using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class PIP: BaseObject
    {
        public Guid EmployeeId { get; set; } //employeeId 
        public Employee Employee { get; set; }
        public string Emp_No { get; set; }
        public string PIPSubject { get; set; }
        public string PIPMessage { get; set; }
        public DateTime DateOfReview { get; set; }
        public string LineManager { get; set; }
        public bool IsSignedOff { get; set; }
        public bool IsClosed { get; set; }
    }

    public class PIPItem: BaseObject
    {
        public Guid PIPId { get; set; }
        public PIP PIP { get; set; }
        public string Comment { get; set; }
        public string PublishBy { get; set; }
    } 
}
