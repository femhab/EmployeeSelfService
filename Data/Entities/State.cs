using Data.Entities.Common;

namespace Data.Entities
{
    public class State: BaseObject
    {
        public string StateCode { get; set; }
        public string Descc { get; set; }
        public string ZoneCode { get; set; }
    }
}
