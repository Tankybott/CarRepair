using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
using DataAccess.Repository.IRepository;
using Service.ServiceRelated;
using Service.ServiceRelated.Interface;
using Service.ServiceTypeRelated;
using Service.ServiceTypeRelated.Interface;

namespace CarRepair.DI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDI(this IServiceCollection services)
        {
            //repo
            services.AddScoped<IServiceTypeRepository, ServiceTypeRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();

            //service
            services.AddScoped<IServiceTypeBase, ServiceTypeBase>();
            services.AddScoped<IServiceTypeReader, ServiceTypeReadService>();
            services.AddScoped<IServiceBase, ServiceBase>();
            services.AddScoped<IServiceReader, ServiceReadService>();

            return services;
        }
    }
}
