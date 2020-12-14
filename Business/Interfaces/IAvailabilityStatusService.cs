using Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IAvailabilityStatusService
    {
        Task<BaseResponse> Refresh();
        Task<IEnumerable<AvalaibilityStatus>> GetAll();
    }
}
