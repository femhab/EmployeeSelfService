using Data.Entities.Common;

namespace Data.Entities
{
    public class Section: BaseObject
    {
        public string SectionCode { get; set; }
        public string CompanyCode { get; set; }
        public string DivisionCode { get; set; }
        public string DeptCode { get; set; }
        public string Descc { get; set; }
        public string SectionAccount { get; set; }
    }
}
