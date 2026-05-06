using Model.DTOs.IntranetDto;

namespace Service.PartRelated.Interface
{
    public interface IPartUpdater
    {
        Task<IntranetPartReadDto> UpdateAsync(IntranetPartUpsertDto dto);
    }
}
