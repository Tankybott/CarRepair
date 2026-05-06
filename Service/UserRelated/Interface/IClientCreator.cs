using Model.DTOs.IntranetDto;

namespace Service.UserRelated.Interface
{
    public interface IClientCreator
    {
        Task<IntranetClientReadDto> CreateAsync(IntranetClientCreateDto dto);
    }
}
