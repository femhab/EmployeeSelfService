using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class ExitProcessPriorityItem : BaseObject
    {
        public Guid ExitProcessId { get; set; }
        public Guid ExitProcess { get; set; }
        public Guid ClearanceDepartmentId { get; set; }
        public Department Department { get; set; }
        public string ClearanceOfficer { get; set; }
        public DateTime EntryDate { get; set; }
        public string Comment { get; set; }
    }
}
