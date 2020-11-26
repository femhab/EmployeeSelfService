using Data.Entities.Common;

namespace Data.Entities
{
    public class AppraisalRating: BaseObject
    {
        public string AppraisalRatingCode { get; set; }
        public string Description { get; set; }
        public int Weight { get; set; }
    }
}
