using System;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class ExitProcessPriorityItemService: IExitProcessPriorityItemService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExitProcessPriorityItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Create(Guid exitProcessId, Guid clearanceDepartmentId, string clearanceManager, string comment)
        {
            var check = await _unitOfWork.GetRepository<ExitProcess>().GetFirstOrDefaultAsync(predicate: x => x.Id == exitProcessId);
            if (check != null)
            {
                var model = new ExitProcessPriorityItem() { ExitProcessId = exitProcessId, ClearanceDepartmentId = clearanceDepartmentId, ClearanceOfficer = clearanceManager, Comment = comment, CreatedDate = DateTime.Now, Id = Guid.NewGuid() };
                _unitOfWork.GetRepository<ExitProcessPriorityItem>().Insert(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.ApprovedSuccessfully };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.NoRecordExist };
        }
    }
}
