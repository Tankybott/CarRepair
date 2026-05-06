using AutoMapper;
using DataAccess.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.ServiceRelated.Interface;

namespace Service.ServiceRelated
{
    public class ServiceBase : IServiceBase
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceBase> _logger;

        public ServiceBase(IServiceRepository serviceRepository, IMapper mapper, ILogger<ServiceBase> logger)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IntranetServiceReadDto> Upsert(IntranetServiceUpsertDto dto)
        {
            if (dto.Id == 0)
            {
                var entity = _mapper.Map<Model.DomainModel.Service>(dto);
                _serviceRepository.Add(entity);
                await _serviceRepository.SaveAsync();

                var created = await _serviceRepository.GetAsync(s => s.Id == entity.Id, false, s => s.ServiceType);
                return _mapper.Map<IntranetServiceReadDto>(created!);
            }
            else
            {
                var entity = await _serviceRepository.GetAsync(s => s.Id == dto.Id, true);

                if (entity == null)
                {
                    _logger.LogError($"Service with id {dto.Id} not found for update.");
                    throw new Exception($"Service with id {dto.Id} not found for update.");
                }

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

        public async Task DeleteAsync(int id)
        {
            var entity = await _serviceRepository.GetAsync(s => s.Id == id);

            if (entity == null)
            {
                _logger.LogError($"Service with id {id} not found for delete.");
                throw new Exception($"Service with id {id} not found.");
            }

            _serviceRepository.Remove(entity);
            await _serviceRepository.SaveAsync();
        }
    }
}
