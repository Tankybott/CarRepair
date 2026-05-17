using Model.DTOs.IntranetDto;

namespace Service.RepairRelated.Interface
{
    public interface IOverheadCostAdder
    {
        Task<IntranetCostEstimationItemReadDto> AddAsync(IntranetOverheadCostUpsertDto dto);
    }
}
