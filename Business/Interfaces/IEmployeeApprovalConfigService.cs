using Data.Entities;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IEmployeeApprovalConfigService
    {
        Task<BaseResponse> Create(EmployeeApprovalConfig model);
        Task<BaseResponse> Edit(EmployeeApprovalConfig model);
        Task<BaseResponse> Reset();
    }
}
