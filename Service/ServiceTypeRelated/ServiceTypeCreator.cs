using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.ServiceTypeRelated.Interface;

namespace Service.ServiceTypeRelated
{
    public class ServiceTypeCreator : IServiceTypeCreator
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IMapper _mapper;

        public ServiceTypeCreator(IServiceTypeRepository serviceTypeRepository, IMapper mapper)
        {
            _serviceTypeRepository = serviceTypeRepository;
            _mapper = mapper;
        }

        public async Task<ServiceTypeDto> CreateAsync(ServiceTypeDto dto)
        {
            var entity = _mapper.Map<ServiceType>(dto);
            _serviceTypeRepository.Add(entity);
            await _serviceTypeRepository.SaveAsync();
            return _mapper.Map<ServiceTypeDto>(entity);
        }
    }
}
