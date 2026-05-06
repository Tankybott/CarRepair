using Model.DTOs.IntranetDto;

namespace Service.ServiceRelated.Interface
{
    public interface IServiceUpdater
    {
        Task<IntranetServiceReadDto> UpdateAsync(IntranetServiceUpsertDto dto);
    }
}
