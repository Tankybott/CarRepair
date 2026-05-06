using Model.DTOs.IntranetDto;

namespace Service.RepairRelated.Interface
{
    public interface IRepairCreator
    {
        Task<IntranetRepairReadDto> CreateAsync(IntranetRepairCreateDto dto);
    }
}
