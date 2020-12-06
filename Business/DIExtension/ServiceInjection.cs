using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Business.Mapping;
using Business.Interfaces;
using Business.Services;
using Business.Providers.JWT;

namespace Business.DIExtension
{
    public static class ServiceInjection
    {
        public static IServiceCollection AddInjectedServices(this IServiceCollection services)
        {
            //automapper
            services.AddAutoMapper(typeof(AutoMapperProfile));

            //services to interfaces
            services.AddTransient<IAuthTokenProvider, AuthTokenProvider>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IAppraisalRatingService, AppraisalRatingService>();
            services.AddTransient<IAppraisalCategoryService, AppraisalCategoryService>();
            services.AddTransient<IAppraisalCategoryItemService, AppraisalCategoryItemService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IEmployeeNOKDetailService, EmployeeNOKDetailService>();
            services.AddTransient<IEmployeeFamilyDependentService, EmployeeFamilyDependentService>();
            services.AddTransient<IEmployeeAddressService, EmployeeAddressService>();
            services.AddTransient<IEmployeeEducationalDetailService, EmployeeEducationalDetailService>();
            services.AddTransient<IEmployeeApprovalConfigService, EmployeeApprovalConfigService>();
            services.AddTransient<IApprovalWorkItemService, ApprovalWorkItemService>();
            services.AddTransient<IApprovalBoardService, ApprovalBoardService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserRoleService, UserRoleService>();
            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<ILeaveService, LeaveService>();
            services.AddTransient<ILeaveTypeService, LeaveTypeService>();
            services.AddTransient<IGradeLevelService, GradeLevelService>();
            services.AddTransient<IRelationshipService, RelationshipService>();
            services.AddTransient<ILoanService, LoanService>();
            services.AddTransient<ILoanTypeService, LoanTypeService>();
            services.AddTransient<IDisciplinaryActionService, DisciplinaryActionService>();
            services.AddTransient<IExitProcessService, ExitProcessService>();
            services.AddTransient<IExitProcessPriorityItemService, ExitProcessPriorityItemService>();


            return services;
        }
    }
}
