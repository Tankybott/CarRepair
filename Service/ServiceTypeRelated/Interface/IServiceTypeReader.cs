using Model.DTOs.IntranetDto;

namespace Service.ServiceTypeRelated.Interface
{
    public interface IServiceTypeReader
    {
        Task<IEnumerable<ServiceTypeDto>> GetAllForIndex();
    }
}