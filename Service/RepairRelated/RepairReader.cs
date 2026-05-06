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
    }
}
