using System;

namespace Data.Entities
{
    public class ApprovalSetting
    {
        public string Name { get; set; } //First Level, Second Level
        public int Level { get; set; } //1, 2
        public Guid Id { get; set; }
    }
}
