using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.RepairRelated.Interface;

namespace Service.RepairRelated
{
    public class RepairReadService : IRepairReader
    {
        private readonly IRepairRepository _repairRepository;
        private readonly IMapper _mapper;

        public RepairReadService(IRepairRepository repairRepository, IMapper mapper)
        {
            _repairRepository = repairRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IntranetRepairReadDto>> GetAllForIndex()
        {
            var repairs = await _repairRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<Repair>, IEnumerable<IntranetRepairReadDto>>(repairs);
        }

        public async Task<IEnumerable<IntranetRepairReadDto>> GetForCarAndUser(int carId, string userId)
        {
            var repairs = await _repairRepository.GetAllForCarAndUserAsync(carId, userId);
            return _mapper.Map<IEnumerable<Repair>, IEnumerable<IntranetRepairReadDto>>(repairs);
        }

        public async Task<IntranetRepairManageDto?> GetManageForUser(int repairId, string userId)
        {
            var repair = await _repairRepository.GetForManageAsync(repairId);
            if (repair == null || repair.Car?.Client?.ApplicationUserId != userId)
                return null;
            return _mapper.Map<IntranetRepairManageDto>(repair);
        }
    }
}
