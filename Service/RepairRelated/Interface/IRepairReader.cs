using Model.DTOs.IntranetDto;

namespace Service.RepairRelated.Interface
{
    public interface IRepairReader
    {
        Task<IEnumerable<IntranetRepairReadDto>> GetAllForIndex();
        Task<IEnumerable<IntranetRepairReadDto>> GetForCarAndUser(int carId, string userId);
        Task<IntranetRepairManageDto?> GetManageForUser(int repairId, string userId);
    }
}
