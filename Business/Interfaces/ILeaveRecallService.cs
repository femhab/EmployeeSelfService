using Data.Entities;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface ILeaveRecallService
    {
        Task<BaseResponse> Create(LeaveRecall model);
    }
}
