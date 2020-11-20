using AutoMapper;

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
            //CreateMap<Department, DepartmentFormModel>().ReverseMap();
        }
    }
}
