using Data.Entities.Common;

namespace Data.Entities
{
    public class EducationalGrade: BaseObject
    {
        public string Code { get; set; } //U002
        public string Description { get; set; } //second class upper
        public string EducationalLevelCode { get; set; }
    }
}
