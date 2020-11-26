using Data.Entities.Common;

namespace Data.Entities
{
    public class AppraisalCategory: BaseObject
    {
        public string AppraisalCategoryCode { get; set; }
        public string Description { get; set; }
        public int OrderNo { get; set; }
    }
}
