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
    public class GradeLevelService : IGradeLevelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GradeLevelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GradeLevel>> GetAll()
        {
            var data = await _unitOfWork.GetRepository<IEnumerable<GradeLevel>>().FindAsync();
            return data;
        }

        public async Task<BaseResponse> Refresh()
        {

        }

    }
}

