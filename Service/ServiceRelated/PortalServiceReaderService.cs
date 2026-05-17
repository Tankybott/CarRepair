using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.PortalDto;
using Service.ServiceRelated.Interface;

namespace Service.ServiceRelated
{
    public class PortalServiceReaderService : IPortalServiceReader
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public PortalServiceReaderService(IServiceRepository serviceRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<CartServiceDto?> GetForBasket(int serviceId)
        {
            var service = await _serviceRepository.GetAsync(s => s.Id == serviceId);
            return service is null ? null : _mapper.Map<Model.DomainModel.Service, CartServiceDto>(service);
        }
    }
}
