using Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IMaritalStatusService
    {
        Task<BaseResponse> Refresh();
        Task<IEnumerable<MaritalStatus>> GetAll();
    }
}
