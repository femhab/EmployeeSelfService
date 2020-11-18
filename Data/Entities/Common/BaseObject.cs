using Data.Enums;
using System;

namespace Data.Entities.Common
{
    public class BaseObject
    {
        public Guid Id { get; set; }
        public string Insertusername { get; set; }
        public DateTime InsertTransacDate { get; set; }
        public DateTime? UpdatedTransacDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
