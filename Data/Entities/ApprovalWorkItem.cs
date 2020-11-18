using Data.Entities.Common;
using Data.Enums;

namespace Data.Entities
{
    public class ApprovalWorkItem: BaseObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
    }
}
