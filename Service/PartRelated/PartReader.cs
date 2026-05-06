using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.PartRelated.Interface;

namespace Service.PartRelated
{
    public class PartReadService : IPartReader
    {
        private readonly IPartRepository _partRepository;
        private readonly IMapper _mapper;

        public PartReadService(IPartRepository partRepository, IMapper mapper)
        {
            _partRepository = partRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IntranetPartReadDto>> GetAllForIndex()
        {
            var parts = await _partRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<Part>, IEnumerable<IntranetPartReadDto>>(parts);
        }
    }
}
