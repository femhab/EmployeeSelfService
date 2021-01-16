using Data.Entities.Common;
using Data.Enums;
using System;

namespace Data.Entities
{
    public class Document: BaseObject
    {
        public Guid RerenceId { get; set; }
        public DocumentType Type { get; set; }
        public string DocumentUrl { get; set; } //not full
    }
}
