using AutoMapper;
using Data.Entities;
using ViewModel.Model;
using ViewModel.ResponseModel;
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
            CreateMap<UnitModel, Unit>().ReverseMap();
            CreateMap<Employee, HRUsers>().ReverseMap(); 
            CreateMap<Employee, EmployeeModel>().ReverseMap(); 
            CreateMap<AppraisalRating, AppraisalRatingModel>().ReverseMap(); 
            CreateMap<AppraisalCategory, AppraisalCategoryModel>().ReverseMap(); 
            CreateMap<AppraisalCategoryItem, AppraisalCategoryItemModel>().ReverseMap();
            CreateMap<LeaveType, LeaveTypeModel>().ReverseMap();
            CreateMap<Leave, LeaveTypeModel>().ReverseMap();
            CreateMap<Leave, LeaveModel>().ReverseMap();
            CreateMap<ExitProcess, ExitProcessModel>().ReverseMap();
            CreateMap<ExitProcessModel, ExitProcess>().ReverseMap();
            CreateMap<ApprovalBoard, ApprovalBaordModel>().ReverseMap();
            CreateMap<AuthDataModel, EmployeeViewModel>().ReverseMap();
            CreateMap<AuthDataModel, EmployeeProfileViewModel>().ReverseMap();
            CreateMap<AuthDataModel, LeaveViewModel>().ReverseMap();
            CreateMap<AuthDataModel, AppraisalViewModel>().ReverseMap();
            CreateMap<AuthDataModel, ApprovalBoardViewModel>().ReverseMap();
            CreateMap<AuthDataModel, DisciplinaryActionViewModel>().ReverseMap();
            CreateMap<AuthDataModel, ExitProcessViewModel>().ReverseMap();
            CreateMap<AuthDataModel, LoanViewModel>().ReverseMap();
            CreateMap<AuthDataModel, DashboardModel>().ReverseMap();
            CreateMap<AuthDataModel, TrainingViewModel>().ReverseMap();
            CreateMap<AuthDataModel, PayrollViewModel>().ReverseMap();
            CreateMap<DashboardResponseModel, DashboardModel>().ReverseMap();
        }
    }
}
