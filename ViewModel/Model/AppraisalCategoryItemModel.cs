using System;

namespace ViewModel.Model
{
    public class AppraisalCategoryItemModel: BaseModel
    {
        public string AppraisalCategoryCode { get; set; }
        public Guid? AppraisalCategoryId { get; set; }
        public string Description { get; set; }
        public string StaffType { get; set; }
        public int Weight { get; set; }
        public int AppraisalCategoryItemID { get; set; }
    }
}
