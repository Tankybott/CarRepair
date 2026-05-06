using DataAccess.Repository.Interfaces;
using Service.ServiceTypeRelated.Interface;

namespace Service.ServiceTypeRelated
{
    public class ServiceTypeDeleter : IServiceTypeDeleter
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;

        public ServiceTypeDeleter(IServiceTypeRepository serviceTypeRepository)
        {
            _serviceTypeRepository = serviceTypeRepository;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _serviceTypeRepository.GetAsync(s => s.Id == id);

            if (entity == null)
                throw new Exception($"ServiceType with id {id} not found.");

            _serviceTypeRepository.Remove(entity);
            await _serviceTypeRepository.SaveAsync();
        }
    }
}
