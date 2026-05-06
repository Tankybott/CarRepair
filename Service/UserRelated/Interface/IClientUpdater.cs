using Model.DTOs.IntranetDto;

namespace Service.UserRelated.Interface
{
    public interface IClientUpdater
    {
        Task<IntranetClientReadDto> UpdateAsync(IntranetClientUpdateDto dto);
    }
}
