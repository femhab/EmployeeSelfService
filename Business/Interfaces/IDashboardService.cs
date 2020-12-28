using System;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardResponseModel> GetDashboard(Guid employeeId);
    }
}
