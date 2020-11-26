using Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IAppraisalCategoryItemService
    {
        Task<BaseResponse> Refresh();
        Task<IEnumerable<AppraisalCategoryItem>> GetAll();
        Task<IEnumerable<AppraisalCategoryItem>> GetByStaffType(string staffType, string appraisalCategoryCode);
    }
}
