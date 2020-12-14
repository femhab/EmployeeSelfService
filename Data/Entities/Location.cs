using Data.Entities.Common;

namespace Data.Entities
{
    public class Location: BaseObject
    {
        public string LocationCode { get; set; }
        public string Descc { get; set; }
        public string StateCode { get; set; }
        public string AccountNo { get; set; }
    }
}
