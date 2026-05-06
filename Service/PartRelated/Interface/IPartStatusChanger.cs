using Model.DomainModel;
using Model.DTOs.IntranetDto;

namespace Service.PartRelated.Interface
{
    public interface IPartStatusChanger
    {
        Task<IntranetPartReadDto> ChangeStatusAsync(int id, PartStatus status);
    }
}
