using Model.DTOs.IntranetDto;

namespace Service.PartRelated.Interface
{
    public interface IPartReader
    {
        Task<IEnumerable<IntranetPartReadDto>> GetAllForIndex();
    }
}
