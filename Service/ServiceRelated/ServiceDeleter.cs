using DataAccess.Repository.Interfaces;
using Service.ServiceRelated.Interface;

namespace Service.ServiceRelated
{
    public class ServiceDeleter : IServiceDeleter
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceDeleter(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _serviceRepository.GetAsync(s => s.Id == id);

            if (entity == null)
                throw new Exception($"Service with id {id} not found.");

            _serviceRepository.Remove(entity);
            await _serviceRepository.SaveAsync();
        }
    }
}
