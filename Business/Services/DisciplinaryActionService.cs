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
    public class DisciplinaryActionService : IDisciplinaryActionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DisciplinaryActionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> CreateQuery(Guid employeeId, string empNo, string subject, string message, Guid initiatorId)
        {
            if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
            {
                var model = new DisciplinaryAction()
                {
                    EmployeeId = employeeId,
                    Emp_No = empNo,
                    QuerySubject = subject,
                    QueryMessage = message,
                    InitiatorId = initiatorId,
                    QueryDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Id = Guid.NewGuid()
                };

                _unitOfWork.GetRepository<DisciplinaryAction>().Insert(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.QueryCreatedSuccessfully };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<DisciplinaryAction>> GetByEmployee(Guid employeeId)
        {
            var data = await GetAll(x => x.EmployeeId == employeeId);
            return data;
        }

        public async Task<IEnumerable<DisciplinaryAction>> GetByInitiator(Guid initiatorId)
        {
            var data = await GetAll(x => x.InitiatorId == initiatorId);
            return data;
        }

        public async Task<BaseResponse> GiveAction(Guid queryId, string actionComment, QueryAction action)
        {
            var check = await _unitOfWork.GetRepository<DisciplinaryAction>().GetFirstOrDefaultAsync(predicate: x => x.Id == queryId);
            if (check != null)
            {
                check.QueryActionComment = actionComment;
                check.QueryActionDate = DateTime.Now;
                check.Action = action;
                check.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<DisciplinaryAction>().Update(check);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.NoRecordExist };
        }

        public async Task<BaseResponse> ReplyQuery(Guid queryId, string reply)
        {
            var check = await _unitOfWork.GetRepository<DisciplinaryAction>().GetFirstOrDefaultAsync(predicate: x => x.Id == queryId);
            if (check != null)
            {
                check.QueryReply = reply;
                check.QueryReplyDate = DateTime.Now;
                check.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<DisciplinaryAction>().Update(check);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.NoRecordExist };
        }

        public async Task<IEnumerable<DisciplinaryAction>> GetAll(Expression<Func<DisciplinaryAction, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<DisciplinaryAction>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<DisciplinaryAction> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<DisciplinaryAction>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
