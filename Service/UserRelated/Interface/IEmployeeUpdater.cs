using Model.DTOs.IntranetDto;

namespace Service.UserRelated.Interface
{
    public interface IEmployeeUpdater
    {
        Task<IntranetEmployeeReadDto> UpdateAsync(IntranetEmployeeUpdateDto dto);
    }
}
