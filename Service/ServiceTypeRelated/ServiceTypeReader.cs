using AutoMapper;
using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
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
    public class ServiceTypeReadService : IServiceTypeReader
    {
        private readonly IMapper _mapper;
        private readonly IServiceTypeRepository _serviceTypeRepository;

        public ServiceTypeReadService(IServiceTypeRepository serviceTypeRepository, IMapper mapper)
        {
            _serviceTypeRepository = serviceTypeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServiceTypeDto>> GetAllForIndex()
        {
            var serviceTypes = await _serviceTypeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ServiceType>, IEnumerable<ServiceTypeDto>>(serviceTypes);
        }


    }
}
