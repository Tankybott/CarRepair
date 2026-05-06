using Model.DTOs.IntranetDto;

namespace Service.WorkTaskRelated.Interface
{
    public interface IWorkTaskUpdater
    {
        Task<IntranetWorkTaskReadDto> UpdateAsync(IntranetWorkTaskUpdateDto dto);
    }
}
