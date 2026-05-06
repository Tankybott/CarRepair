using Model.DTOs.IntranetDto;

namespace Service.UserRelated.Interface
{
    public interface IEmployeeCreator
    {
        Task<IntranetEmployeeReadDto> CreateAsync(IntranetEmployeeCreateDto dto);
    }
}
