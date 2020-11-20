using Data.Entities.Common;

namespace Data.Entities
{
    public class Unit: BaseObject
    {
        public string UnitCode { get; set; }
        public string Descc { get; set; }
        public string DeptCode { get; set; }
        public string CompanyCode { get; set; }
        public string DivisionCode { get; set; }   
        public int Slot { get; set; }
    }
}
