using Model.DTOs.PortalDto;

namespace CarRepair.Services
{
    public interface ICartService
    {
        List<CartServiceDto> GetCart(ISession session);
        void AddService(ISession session, CartServiceDto service);
        void RemoveService(ISession session, int serviceId);
        void Clear(ISession session);
    }
}
