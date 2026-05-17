using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.RepairRelated.Interface;

namespace Service.RepairRelated
{
    public class OverheadCostAdder : IOverheadCostAdder
    {
        private readonly ICostEstimationItemRepository _repo;
        private readonly IMapper _mapper;

        public OverheadCostAdder(ICostEstimationItemRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IntranetCostEstimationItemReadDto> AddAsync(IntranetOverheadCostUpsertDto dto)
        {
            var item = new CostEstimationItem
            {
                RepairId = dto.RepairId,
                Type = CostEstimationItemType.Overhead,
                Name = dto.Name,
                Description = dto.Description,
                Cost = dto.Cost
            };
            _repo.Add(item);
            await _repo.SaveAsync();
            return _mapper.Map<IntranetCostEstimationItemReadDto>(item);
        }
    }
}
