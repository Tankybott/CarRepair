using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.PartRelated.Interface;

namespace Service.PartRelated
{
    public class PartCreator : IPartCreator
    {
        private readonly IPartRepository _partRepository;
        private readonly IMapper _mapper;

        public PartCreator(IPartRepository partRepository, IMapper mapper)
        {
            _partRepository = partRepository;
            _mapper = mapper;
        }

        public async Task<IntranetPartReadDto> CreateAsync(IntranetPartUpsertDto dto)
        {
            var entity = _mapper.Map<Part>(dto);
            _partRepository.Add(entity);
            await _partRepository.SaveAsync();
            return _mapper.Map<IntranetPartReadDto>(entity);
        }
    }
}
