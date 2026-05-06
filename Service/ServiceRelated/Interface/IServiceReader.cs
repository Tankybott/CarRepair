using Model.DTOs.IntranetDto;

namespace Service.ServiceRelated.Interface
{
    public interface IServiceReader
    {
        Task<IEnumerable<IntranetServiceReadDto>> GetAllForIndex();
    }
}
