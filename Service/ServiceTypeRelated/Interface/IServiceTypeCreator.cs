using Model.DTOs.IntranetDto;

namespace Service.ServiceTypeRelated.Interface
{
    public interface IServiceTypeCreator
    {
        Task<ServiceTypeDto> CreateAsync(ServiceTypeDto dto);
    }
}
