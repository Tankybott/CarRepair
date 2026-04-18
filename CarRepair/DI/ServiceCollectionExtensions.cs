using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
using DataAccess.Repository.IRepository;
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

            //service
            //Service Type
            services.AddScoped<IServiceTypeBase, ServiceTypeBase>();
            services.AddScoped<IServiceTypeReader, ServiceTypeReadService>();   

            return services;
        }
    }
}
