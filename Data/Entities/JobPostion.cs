using Data.Entities.Common;

namespace Data.Entities
{
    public class JobPostion: BaseObject
    {
        public string PositionCode { get; set; }
        public string Descc { get; set; }
        public int Slot { get; set; }
    }
}
