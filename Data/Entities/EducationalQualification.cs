using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class EducationalQualification: BaseObject
    {
        public string Code { get; set; } //BSc, MSc
        public string Description { get; set; } //Bachelor, Masters
        public Guid EducationalLevelId { get; set; } //foriegn key
        public EducationalLevel EducationalLevel { get; set; }
    }
}
