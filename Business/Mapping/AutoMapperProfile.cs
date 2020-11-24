﻿using AutoMapper;
using Data.Entities;
using ViewModel.Model;
using ViewModel.ServiceModel;

namespace Business.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            Config();
        }
        public void Config()
        {
            CreateMap<HRUsers, HRUserModel>().ReverseMap();
            CreateMap<HRUserModel, HRUsers>().ReverseMap();
            CreateMap<RoleModel, Role>().ReverseMap();
            CreateMap<DepartmentModel, Department>().ReverseMap();
            CreateMap<Employee, HRUsers>().ReverseMap();
        }
    }
}
