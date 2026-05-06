using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.ServiceRelated.Interface;

namespace Service.ServiceRelated
{
    public class ServiceReadService : IServiceReader
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public ServiceReadService(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IntranetServiceReadDto>> GetAllForIndex()
        {
            var services = await _serviceRepository.GetAllAsync(null, false, s => s.ServiceType);
            return _mapper.Map<IEnumerable<Model.DomainModel.Service>, IEnumerable<IntranetServiceReadDto>>(services);
        }
    }
}
