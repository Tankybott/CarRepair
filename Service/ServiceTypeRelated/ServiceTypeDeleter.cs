using DataAccess.Repository.Interfaces;
using Service.ServiceTypeRelated.Interface;

namespace Service.ServiceTypeRelated
{
    public class ServiceTypeDeleter : IServiceTypeDeleter
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IServiceRepository _serviceRepository;

        public ServiceTypeDeleter(IServiceTypeRepository serviceTypeRepository, IServiceRepository serviceRepository)
        {
            _serviceTypeRepository = serviceTypeRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _serviceTypeRepository.GetAsync(s => s.Id == id);

            if (entity == null)
                throw new Exception($"ServiceType with id {id} not found.");

            if (_serviceRepository.Any(s => s.ServiceTypeId == id && s.DeletedAt == null))
                throw new InvalidOperationException("Cannot delete a service type that has active services assigned to it.");

            _serviceTypeRepository.Remove(entity);
            await _serviceTypeRepository.SaveAsync();
        }
    }
}
