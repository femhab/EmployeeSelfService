using Data.Entities.Common;
using System;

namespace Data.Entities
{
    public class AppraisalCategoryItem: BaseObject
    {
        public string AppraisalCategoryCode { get; set; }
        public Guid? AppraisalCategoryId { get; set; }
        public AppraisalCategory AppraisalCategory { get; set; }
        public string Description { get; set; }
        public string StaffType { get; set; }
        public int Weight { get; set; }
        public int AppraisalCategoryItemID { get; set; }
    }
}
