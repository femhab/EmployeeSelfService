using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Business.Mapping;

namespace Business.DIExtension
{
    public static class ServiceInjection
    {
        public static IServiceCollection AddInjectedServices(this IServiceCollection services)
        {
            //automapper
            services.AddAutoMapper(typeof(AutoMapperProfile));

            //services to interfaces
            //services.AddTransient<IAuthTokenProvider, AuthTokenProvider>();
            

            return services;
        }
    }
}
