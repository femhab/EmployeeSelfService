using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class Notification: BaseObject
    {
        public Guid? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsGeneral { get; set; }
    }
}
