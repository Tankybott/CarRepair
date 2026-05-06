using Model.DTOs.IntranetDto;

namespace Service.ServiceTypeRelated.Interface
{
    public interface IServiceTypeUpdater
    {
        Task<ServiceTypeDto> UpdateAsync(ServiceTypeDto dto);
    }
}
