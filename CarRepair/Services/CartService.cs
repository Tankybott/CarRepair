using Model.DTOs.PortalDto;
using System.Text.Json;

namespace CarRepair.Services
{
    public class CartService : ICartService
    {
        private const string SessionKey = "cart";

        public List<CartServiceDto> GetCart(ISession session)
        {
            var json = session.GetString(SessionKey);
            return json is null
                ? new List<CartServiceDto>()
                : JsonSerializer.Deserialize<List<CartServiceDto>>(json)!;
        }

        public void AddService(ISession session, CartServiceDto service)
        {
            var basket = GetCart(session);
            if (!basket.Any(s => s.Id == service.Id))
                basket.Add(service);
            Save(session, basket);
        }

        public void RemoveService(ISession session, int serviceId)
        {
            var basket = GetCart(session);
            basket.RemoveAll(s => s.Id == serviceId);
            Save(session, basket);
        }

        public void Clear(ISession session)
        {
            session.Remove(SessionKey);
        }

        private static void Save(ISession session, List<CartServiceDto> basket)
        {
            session.SetString(SessionKey, JsonSerializer.Serialize(basket));
        }
    }
}
