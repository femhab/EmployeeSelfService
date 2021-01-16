using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class DocumentService: IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DocumentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> CreateOrUpdate(Document model)
        {
            var photo = await GetByReference(model.RerenceId, model.Type);
            if (photo != null)
            {
                photo.DocumentUrl = model.DocumentUrl;
                _unitOfWork.GetRepository<Document>().Update(photo);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
            }
            else
            {
                _unitOfWork.GetRepository<Document>().Insert(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<Document> GetByReference(Guid referenceId, DocumentType type)
        {
            try
            {
                var data = await _unitOfWork.GetRepository<Document>().GetFirstOrDefaultAsync(predicate: x => x.RerenceId == referenceId && x.Type == type);
                return data;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
