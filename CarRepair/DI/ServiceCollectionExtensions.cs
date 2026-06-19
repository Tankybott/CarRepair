using CarRepair.Services;
using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
using Service.CarRelated;
using Service.CarRelated.Interface;
using Service.PartRelated;
using Service.PartRelated.Interface;
using Service.RepairRelated;
using Service.RepairRelated.Interface;
using Service.ServiceRelated;
using Service.WorkTaskRelated;
using Service.WorkTaskRelated.Interface;
using Service.ServiceRelated.Interface;
using Service.ServiceTypeRelated;
using Service.ServiceTypeRelated.Interface;
using Service.UserRelated;
using Service.UserRelated.Interface;
using Service.WebsiteConfigRelated;
using Service.WebsiteConfigRelated.Interface;

namespace CarRepair.DI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDI(this IServiceCollection services)
        {
            // repositories
            services.AddScoped<IRepairRepository, RepairRepository>();
            services.AddScoped<IServiceTypeRepository, ServiceTypeRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IPartRepository, PartRepository>();
            services.AddScoped<IClientProfileRepository, ClientProfileRepository>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<ICostEstimationItemRepository, CostEstimationItemRepository>();
            services.AddScoped<IWorkTaskRepository, WorkTaskRepository>();
            services.AddScoped<IEmployeeBookingRepository, EmployeeBookingRepository>();
            services.AddScoped<IRepairBookingRepository, RepairBookingRepository>();

            // portal
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOfferReader, OfferReaderService>();
            services.AddScoped<IPortalServiceReader, PortalServiceReaderService>();



            // service type
            services.AddScoped<IServiceTypeCreator, ServiceTypeCreator>();
            services.AddScoped<IServiceTypeUpdater, ServiceTypeUpdater>();
            services.AddScoped<IServiceTypeDeleter, ServiceTypeDeleter>();
            services.AddScoped<IServiceTypeReader, ServiceTypeReadService>();

            // service
            services.AddScoped<IServiceCreator, ServiceCreator>();
            services.AddScoped<IServiceUpdater, ServiceUpdater>();
            services.AddScoped<IServiceDeleter, ServiceDeleter>();
            services.AddScoped<IServiceReader, ServiceReadService>();

            // part
            services.AddScoped<IPartCreator, PartCreator>();
            services.AddScoped<IPartUpdater, PartUpdater>();
            services.AddScoped<IPartDeleter, PartDeleter>();
            services.AddScoped<IPartStatusChanger, PartStatusChanger>();
            services.AddScoped<IPartReader, PartReadService>();

            // client
            services.AddScoped<IClientCreator, ClientCreator>();
            services.AddScoped<IClientUpdater, ClientUpdater>();
            services.AddScoped<IClientDeleter, ClientDeleter>();
            services.AddScoped<IClientReader, ClientReadService>();

            // repair
            services.AddScoped<IRepairCreator, RepairCreator>();
            services.AddScoped<IRepairReader, RepairReadService>();
            services.AddScoped<IRepairDeleter, RepairDeleter>();
            services.AddScoped<ICostEstimationCreator, CostEstimationCreator>();
            services.AddScoped<IRepairServiceAdder, RepairServiceAdder>();
            services.AddScoped<IRepairStatusUpdater, RepairStatusUpdater>();
            services.AddScoped<IOverheadCostAdder, OverheadCostAdder>();
            services.AddScoped<IOverheadCostDeleter, OverheadCostDeleter>();
            services.AddScoped<IWorkTaskCreator, WorkTaskCreator>();
            services.AddScoped<IWorkTaskUpdater, WorkTaskUpdater>();
            services.AddScoped<IWorkTaskDeleter, WorkTaskDeleter>();
            services.AddScoped<IRepairScheduler, RepairScheduler>();

            // employee
            services.AddScoped<IEmployeeProfileRepository, EmployeeProfileRepository>();
            services.AddScoped<IEmployeeCreator, EmployeeCreator>();
            services.AddScoped<IEmployeeUpdater, EmployeeUpdater>();
            services.AddScoped<IEmployeeDeleter, EmployeeDeleter>();
            services.AddScoped<IEmployeeReader, EmployeeReadService>();
            services.AddScoped<IEmployeeAvailabilityGetter, EmployeeAvailabilityGetter>();

            // car
            services.AddScoped<ICarCreator, CarCreator>();
            services.AddScoped<ICarUpdater, CarUpdater>();
            services.AddScoped<ICarDeleter, CarDeleter>();
            services.AddScoped<ICarReader, CarReadService>();

            // website config
            services.AddScoped<IWebsiteConfigRepository, WebsiteConfigRepository>();
            services.AddScoped<IWebsiteConfigReader, WebsiteConfigReader>();
            services.AddScoped<IWebsiteConfigUpdater, WebsiteConfigUpdater>();
            services.AddScoped<IWorkingHoursValidator, WorkingHoursValidator>();

            return services;
        }
    }
}
