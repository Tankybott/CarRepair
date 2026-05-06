using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DTOs.IntranetDto;
using Service.ServiceRelated.Interface;

namespace Service.ServiceRelated
{
    public class ServiceCreator : IServiceCreator
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public ServiceCreator(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<IntranetServiceReadDto> CreateAsync(IntranetServiceUpsertDto dto)
        {
            var entity = _mapper.Map<Model.DomainModel.Service>(dto);
            _serviceRepository.Add(entity);
            await _serviceRepository.SaveAsync();

            var created = await _serviceRepository.GetAsync(s => s.Id == entity.Id, false, s => s.ServiceType);
            return _mapper.Map<IntranetServiceReadDto>(created!);
        }
    }
}
