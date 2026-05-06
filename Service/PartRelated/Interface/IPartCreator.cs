using Model.DTOs.IntranetDto;

namespace Service.PartRelated.Interface
{
    public interface IPartCreator
    {
        Task<IntranetPartReadDto> CreateAsync(IntranetPartUpsertDto dto);
    }
}
