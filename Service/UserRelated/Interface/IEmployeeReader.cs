using Model.DTOs.IntranetDto;

namespace Service.UserRelated.Interface
{
    public interface IEmployeeReader
    {
        Task<IEnumerable<IntranetEmployeeReadDto>> GetAllForIndex();
    }
}
