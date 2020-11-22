using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Business.Interfaces;
using Data.Entities;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Refresh()
        {

        }

        public async Task<IEnumerable<Department>> GetAll()
        {
            var data = await _unitOfWork.GetRepository<IEnumerable<Department>>().FindAsync();
            return data;
        }

       

    }
}
