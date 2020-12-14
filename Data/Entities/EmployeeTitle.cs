using Data.Entities.Common;

namespace Data.Entities
{
    public class EmployeeTitle: BaseObject
    {
        public string TitleCode { get; set; }
        public int Slot { get; set; }
        public string Descc { get; set; }
    }
}
