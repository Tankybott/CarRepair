using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DTOs.IntranetDto;
using Service.ServiceTypeRelated.Interface;

namespace Service.ServiceTypeRelated
{
    public class ServiceTypeUpdater : IServiceTypeUpdater
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IMapper _mapper;

        public ServiceTypeUpdater(IServiceTypeRepository serviceTypeRepository, IMapper mapper)
        {
            _serviceTypeRepository = serviceTypeRepository;
            _mapper = mapper;
        }

        public async Task<ServiceTypeDto> UpdateAsync(ServiceTypeDto dto)
        {
            var entity = await _serviceTypeRepository.GetAsync(s => s.Id == dto.Id);

            if (entity == null)
                throw new Exception($"ServiceType with id {dto.Id} not found.");

            entity.Name = dto.Name;
            entity.Description = dto.Description;

            _serviceTypeRepository.Update(entity);
            await _serviceTypeRepository.SaveAsync();

            return _mapper.Map<ServiceTypeDto>(entity);
        }
    }
}
