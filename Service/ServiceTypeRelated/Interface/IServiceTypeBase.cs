using Model.DTOs.IntranetDto;

namespace Service.ServiceTypeRelated.Interface
{
    public interface IServiceTypeBase
    {
        Task DeleteAsync(int id);
        Task<ServiceTypeDto> Upsert(ServiceTypeDto dto);
    }
}