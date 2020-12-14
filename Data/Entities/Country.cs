using Data.Entities.Common;

namespace Data.Entities
{
    public class Country: BaseObject
    {
        public string CountryCode { get; set; }
        public string Descc { get; set; }
    }
}
