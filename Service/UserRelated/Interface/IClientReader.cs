using Model.DTOs.IntranetDto;

namespace Service.UserRelated.Interface
{
    public interface IClientReader
    {
        Task<IEnumerable<IntranetClientReadDto>> GetAllForIndex();
    }
}
