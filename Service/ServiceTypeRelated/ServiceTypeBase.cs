using AutoMapper;
using DataAccess.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.ServiceTypeRelated.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ServiceTypeRelated
{
    public class ServiceTypeBase : IServiceTypeBase
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceTypeBase> _logger;

        public ServiceTypeBase(IServiceTypeRepository serviceTypeRepository, IMapper mapper, ILogger<ServiceTypeBase> logger)
        {
            _serviceTypeRepository = serviceTypeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ServiceTypeDto> Upsert(ServiceTypeDto dto)
        {
            if (dto.Id == 0)
            {
                var entity = _mapper.Map<ServiceType>(dto);
                _serviceTypeRepository.Add(entity);
                await _serviceTypeRepository.SaveAsync();

                return _mapper.Map<ServiceTypeDto>(entity);
            }
            else
            {
                var entity = await _serviceTypeRepository.GetAsync(s => s.Id == dto.Id);

                if (entity == null)
                {
                    _logger.LogError($"ServiceType with id {dto.Id} not found for update.");
                    throw new Exception($"ServiceType with id {dto.Id} not found for update.");
                }

                entity.Name = dto.Name;
                entity.Description = dto.Description;

                _serviceTypeRepository.Update(entity);
                await _serviceTypeRepository.SaveAsync();

                return _mapper.Map<ServiceTypeDto>(entity);
            }
        }



        public async Task DeleteAsync(ServiceType serviceType)
        {
            _serviceTypeRepository.Remove(serviceType);
            await _serviceTypeRepository.SaveAsync();
        }
    }
}
