using Model.DTOs.PortalDto;

namespace Service.ServiceTypeRelated.Interface
{
    public interface IOfferReader
    {
        Task<IEnumerable<OfferServiceTypeDto>> GetAllForOffer();
    }
}
