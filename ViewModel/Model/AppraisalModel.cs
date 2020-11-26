using System.Collections.Generic;

namespace ViewModel.Model
{
    public class AppraisalViewModel
    {
        public IEnumerable<AppraisalRatingModel> AppraisalRatings { get; set; }
        public IEnumerable<AppraisalCategoryModel> AppraisalCategories { get; set; }
        public IEnumerable<AppraisalCategoryItemModel> AppraisalCategoryItems { get; set; }
        public IEnumerable<EmployeeModel> EmployeeList { get; set; }
    }
}
