using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.PortalDto;
using Service.ServiceTypeRelated.Interface;

namespace Service.ServiceTypeRelated
{
    public class OfferReaderService : IOfferReader
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IMapper _mapper;

        public OfferReaderService(IServiceTypeRepository serviceTypeRepository, IMapper mapper)
        {
            _serviceTypeRepository = serviceTypeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OfferServiceTypeDto>> GetAllForOffer()
        {
            var serviceTypes = await _serviceTypeRepository.GetAllAsync(null, false, st => st.Services);
            return _mapper.Map<IEnumerable<ServiceType>, IEnumerable<OfferServiceTypeDto>>(serviceTypes);
        }
    }
}
