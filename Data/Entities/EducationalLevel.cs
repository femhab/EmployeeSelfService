using Data.Entities.Common;

namespace Data.Entities
{
    public class EducationalLevel: BaseObject
    {
        public string Code { get; set; } //PRI, UNI
        public string Description { get; set; } //primary, university
    }
}
