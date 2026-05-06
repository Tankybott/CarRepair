using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DTOs.IntranetDto;
using Service.ServiceRelated.Interface;

namespace Service.ServiceRelated
{
    public class ServiceUpdater : IServiceUpdater
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public ServiceUpdater(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<IntranetServiceReadDto> UpdateAsync(IntranetServiceUpsertDto dto)
        {
            var entity = await _serviceRepository.GetAsync(s => s.Id == dto.Id, true);

            if (entity == null)
                throw new Exception($"Service with id {dto.Id} not found.");

            entity.ServiceTypeId = dto.ServiceTypeId;
            entity.ShortDescription = dto.ShortDescription;
            entity.Description = dto.Description;
            entity.AveragePrice = dto.AveragePrice;

            _serviceRepository.Update(entity);
            await _serviceRepository.SaveAsync();

            var updated = await _serviceRepository.GetAsync(s => s.Id == entity.Id, false, s => s.ServiceType);
            return _mapper.Map<IntranetServiceReadDto>(updated!);
        }
    }
}
