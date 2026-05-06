using Model.DTOs.IntranetDto;

namespace Service.WorkTaskRelated.Interface
{
    public interface IWorkTaskCreator
    {
        Task<IntranetWorkTaskReadDto> CreateAsync(IntranetWorkTaskCreateDto dto);
    }
}
