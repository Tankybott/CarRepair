using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DTOs.IntranetDto;
using Service.PartRelated.Interface;

namespace Service.PartRelated
{
    public class PartUpdater : IPartUpdater
    {
        private readonly IPartRepository _partRepository;
        private readonly IMapper _mapper;

        public PartUpdater(IPartRepository partRepository, IMapper mapper)
        {
            _partRepository = partRepository;
            _mapper = mapper;
        }

        public async Task<IntranetPartReadDto> UpdateAsync(IntranetPartUpsertDto dto)
        {
            var entity = await _partRepository.GetAsync(p => p.Id == dto.Id, true);

            if (entity == null)
                throw new Exception($"Part with id {dto.Id} not found.");

            entity.RepairId = dto.RepairId;
            entity.Name = dto.Name;
            entity.SerialNumber = dto.SerialNumber;
            entity.Manufacturer = dto.Manufacturer;
            entity.Description = dto.Description;
            entity.UnitPrice = dto.UnitPrice;
            entity.Quantity = dto.Quantity;

            _partRepository.Update(entity);
            await _partRepository.SaveAsync();

            return _mapper.Map<IntranetPartReadDto>(entity);
        }
    }
}
