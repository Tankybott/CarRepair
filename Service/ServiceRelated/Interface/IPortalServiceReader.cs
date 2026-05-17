using Model.DTOs.PortalDto;

namespace Service.ServiceRelated.Interface
{
    public interface IPortalServiceReader
    {
        Task<CartServiceDto?> GetForBasket(int serviceId);
    }
}
