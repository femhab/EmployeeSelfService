using System.Collections.Generic;

namespace ViewModel.Model
{
    public class AppraisalViewModel: AuthDataModel
    {
        public IEnumerable<AppraisalRatingModel> AppraisalRatings { get; set; }
        public IEnumerable<AppraisalCategoryModel> AppraisalCategories { get; set; }
        public IEnumerable<AppraisalCategoryItemModel> AppraisalCategoryItems { get; set; }
        public IEnumerable<EmployeeModel> EmployeeList { get; set; }
        public IEnumerable<EmployeeAppraisalModel> EmployeeAppraisal { get; set; }
        public IEnumerable<AppraisalItemModel> AppraisalItem { get; set; }
        public EmployeeModel Employee { get; set; }
    }
}
