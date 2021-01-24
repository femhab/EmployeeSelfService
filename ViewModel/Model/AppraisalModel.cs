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
        public EmployeeAppraisalModel TargetAppraisal { get; set; }
        public ApprovalBaordModel TargetBoard { get; set; }
        public IEnumerable<AppraisalItemModel> AppraisalItem { get; set; }
        public IEnumerable<ContractItemModel> ContractItem { get; set; }
        public ContractObjectiveModel ContractObjective { get; set; }
        public EmployeeModel Employee { get; set; }
        public bool IsContractible { get; set; }
    }
}
