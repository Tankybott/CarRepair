using Model.DomainModel;
using Model.DTOs.IntranetDto;

namespace Service.ServiceTypeRelated.Interface
{
    public interface IServiceTypeBase
    {
        Task DeleteAsync(ServiceType serviceType);
        Task<ServiceTypeDto> Upsert(ServiceTypeDto dto);
    }
}