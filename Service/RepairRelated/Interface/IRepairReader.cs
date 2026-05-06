using Model.DTOs.IntranetDto;

namespace Service.RepairRelated.Interface
{
    public interface IRepairReader
    {
        Task<IEnumerable<IntranetRepairReadDto>> GetAllForIndex();
    }
}
