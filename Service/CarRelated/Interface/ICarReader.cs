using Model.DTOs.IntranetDto;
using Model.DTOs.PortalDto;

namespace Service.CarRelated.Interface
{
    public interface ICarReader
    {
        Task<IEnumerable<IntranetCarReadDto>> GetAllForIndex();
        Task<IEnumerable<PortalCarDto>> GetCarsForUser(string userId);
        Task<IEnumerable<CarAndRepairCarDto>> GetCarsDetailForUser(string userId);
        Task<CarAndRepairCarDto?> GetCarForUser(int carId, string userId);
    }
}
