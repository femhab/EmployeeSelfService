using Data.Entities.Common;

namespace Data.Entities
{
    public class LGA: BaseObject
    {
        public string LGACode { get; set; }
        public string Descc { get; set; }
        public string StateCode { get; set; }
    }
}
