using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.PartRelated.Interface;

namespace Service.PartRelated
{
    public class PartStatusChanger : IPartStatusChanger
    {
        private readonly IPartRepository _partRepository;
        private readonly IMapper _mapper;

        public PartStatusChanger(IPartRepository partRepository, IMapper mapper)
        {
            _partRepository = partRepository;
            _mapper = mapper;
        }

        public async Task<IntranetPartReadDto> ChangeStatusAsync(int id, PartStatus status)
        {
            var entity = await _partRepository.GetAsync(p => p.Id == id, true);

            if (entity == null)
                throw new Exception($"Part with id {id} not found.");

            entity.Status = status;

            if (status == PartStatus.Delivered && entity.DeliveredAt == null)
                entity.DeliveredAt = DateTime.UtcNow;

            if (status == PartStatus.Installed && entity.InstalledAt == null)
                entity.InstalledAt = DateTime.UtcNow;

            _partRepository.Update(entity);
            await _partRepository.SaveAsync();

            return _mapper.Map<IntranetPartReadDto>(entity);
        }
    }
}
