using Data.Entities.Common;

namespace Data.Entities
{
    public class GradeLevel: BaseObject
    {
        public string GradeCode { get; set; }
        public string Descc { get; set; }
        public int Slot { get; set; }
        public string UserName { get; set; }
    }
}
