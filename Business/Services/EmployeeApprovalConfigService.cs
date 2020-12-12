using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class EmployeeApprovalConfigService: IEmployeeApprovalConfigService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeApprovalConfigService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> CreateUpdate(List<EmployeeApprovalConfig> model)
        {
            if(model != null)
            {
                foreach (var item in model)
                {
                    var check = await _unitOfWork.GetRepository<EmployeeApprovalConfig>().GetFirstOrDefaultAsync(predicate: x => x.ApprovalLevel == item.ApprovalLevel && x.EmployeeId == item.EmployeeId && x.ApprovalWorkItemId == item.ApprovalWorkItemId);
                    if (check == null)
                    {
                        _unitOfWork.GetRepository<EmployeeApprovalConfig>().Insert(model);
                    }
                    else
                    {
                        check.ProcessorIId = item.ProcessorIId;
                        check.UpdatedDate = DateTime.Now;

                        _unitOfWork.GetRepository<EmployeeApprovalConfig>().Update(check);
                    }
                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<EmployeeApprovalConfig>> GetByEmployee(Guid employeeId)
        {
            var data = await GetAll(x => x.EmployeeId == employeeId, "Employee,ApprovalWorkItem");
            return data;
        }

        public async Task<EmployeeApprovalConfig> GetByServiceLevel(Guid employeeId, string approverWorkItem, Level level)
        {
            var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains(approverWorkItem));
            var data = await _unitOfWork.GetRepository<EmployeeApprovalConfig>().GetFirstOrDefaultAsync(predicate: c => c.EmployeeId == employeeId && c.ApprovalWorkItemId == approvalWorkItem.Id && c.ApprovalLevel == level, null, include: e => e.Include(i => i.Employee));
            return data;
        }

        public async Task<EmployeeApprovalConfig> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<EmployeeApprovalConfig>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }

        public async Task<EmployeeApprovalConfig> GetBy(Expression<Func<EmployeeApprovalConfig, bool>> predicate)
        {
            var model = await _unitOfWork.GetRepository<EmployeeApprovalConfig>().GetFirstOrDefaultAsync(predicate: predicate, null, include: e => e.Include(i => i.Employee));
            return model;
        }

        public async Task<BaseResponse> SetApprovalCount(EmployeeApprovalCount model)
        {
            var check = await _unitOfWork.GetRepository<EmployeeApprovalCount>().GetFirstOrDefaultAsync(predicate: x => x.ApprovalWorkItemId == model.ApprovalWorkItemId && x.EmployeeId == model.EmployeeId);
            if (check == null)
            {
                _unitOfWork.GetRepository<EmployeeApprovalCount>().Insert(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            else
            {
                check.MaximumCount = model.MaximumCount;
                check.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<EmployeeApprovalCount>().Update(check);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
            }
        }

        public async Task<int> GetApprovalCount(Guid employeeId, Guid approvalWorkItemId)
        {
            var model = await _unitOfWork.GetRepository<EmployeeApprovalCount>().GetFirstOrDefaultAsync(predicate: c => c.EmployeeId == employeeId && c.ApprovalWorkItemId == approvalWorkItemId);
            return model.MaximumCount;
        }

        public async Task<IEnumerable<EmployeeApprovalConfig>> GetAll(Expression<Func<EmployeeApprovalConfig, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<EmployeeApprovalConfig>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }
    }
}
