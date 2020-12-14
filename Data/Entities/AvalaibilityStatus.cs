using Data.Entities.Common;

namespace Data.Entities
{
    public class AvalaibilityStatus: BaseObject
    {
        public string StatusCode { get; set; }
        public string Descc { get; set; }
        public string Active { get; set; }
    }
}
