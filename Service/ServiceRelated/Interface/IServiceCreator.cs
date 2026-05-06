using Model.DTOs.IntranetDto;

namespace Service.ServiceRelated.Interface
{
    public interface IServiceCreator
    {
        Task<IntranetServiceReadDto> CreateAsync(IntranetServiceUpsertDto dto);
    }
}
