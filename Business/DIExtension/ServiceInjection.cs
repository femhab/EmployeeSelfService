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
            services.AddTransient<IAppraisalPeriodService, AppraisalPeriodService>();
            services.AddTransient<IAppraisalItemService, AppraisalItemService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IEmployeeNOKDetailService, EmployeeNOKDetailService>();
            services.AddTransient<IEmployeeFamilyDependentService, EmployeeFamilyDependentService>();
            services.AddTransient<IEmployeeAddressService, EmployeeAddressService>();
            services.AddTransient<IEmployeeEducationalDetailService, EmployeeEducationalDetailService>();
            services.AddTransient<IEducationalGradeService, EducationalGradeService>();
            services.AddTransient<IEducationalLevelService, EducationalLevelService>();
            services.AddTransient<IEducationalQualificationService, EducationalQualificationService>();
            services.AddTransient<IEmployeeAppraisalService, EmployeeAppraisalService>();
            services.AddTransient<IEmployeeApprovalConfigService, EmployeeApprovalConfigService>();
            services.AddTransient<IApprovalBoardActiveLevelService, ApprovalBoardActiveLevelService>();
            services.AddTransient<IApprovalWorkItemService, ApprovalWorkItemService>();
            services.AddTransient<IApprovalBoardService, ApprovalBoardService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserRoleService, UserRoleService>();
            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<ILeaveService, LeaveService>();
            services.AddTransient<ILeaveRecallService, LeaveRecallService>();
            services.AddTransient<ILeaveTypeService, LeaveTypeService>();
            services.AddTransient<IGradeLevelService, GradeLevelService>();
            services.AddTransient<IRelationshipService, RelationshipService>();
            services.AddTransient<ILoanService, LoanService>();
            services.AddTransient<ILoanTypeService, LoanTypeService>();
            services.AddTransient<IDisciplinaryActionService, DisciplinaryActionService>();
            services.AddTransient<IExitProcessService, ExitProcessService>();
            services.AddTransient<IExitProcessPriorityItemService, ExitProcessPriorityItemService>();
            services.AddTransient<IDivisionService, DivisionService>();
            services.AddTransient<ISectionService, SectionService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IEmployeeTitleService, EmployeeTitleService>();
            services.AddTransient<ICourtesyService, CourtesyService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<IStateService, StateService>();
            services.AddTransient<ILGAService, LGAService>();
            services.AddTransient<IAvailabilityStatusService, AvailabilityStatusService>();
            services.AddTransient<IMaritalStatusService, MaritalStatusService>();
            services.AddTransient<IRefreshingService, RefreshingService>();


            return services;
        }
    }
}
