using Model.DTOs.IntranetDto;

namespace Service.ServiceRelated.Interface
{
    public interface IServiceBase
    {
        Task DeleteAsync(int id);
        Task<IntranetServiceReadDto> Upsert(IntranetServiceUpsertDto dto);
    }
}
