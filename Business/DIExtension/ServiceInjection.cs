using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Business.Mapping;
using Business.Interfaces;
using Business.Services;

namespace Business.DIExtension
{
    public static class ServiceInjection
    {
        public static IServiceCollection AddInjectedServices(this IServiceCollection services)
        {
            //automapper
            services.AddAutoMapper(typeof(AutoMapperProfile));

            //services to interfaces
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IApprovalBoardService, ApprovalBoardService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IDepartmentService, DepartmentService>();


            return services;
        }
    }
}
