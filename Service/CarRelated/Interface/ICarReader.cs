using Model.DTOs.IntranetDto;

namespace Service.CarRelated.Interface
{
    public interface ICarReader
    {
        Task<IEnumerable<IntranetCarReadDto>> GetAllForIndex();
    }
}
