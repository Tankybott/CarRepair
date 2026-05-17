using Model.DTOs.PortalDto;

namespace Model.ViewModel
{
    public class CartViewModel
    {
        public List<CartServiceDto> Services { get; set; } = new();
        public List<PortalCarDto> Cars { get; set; } = new();
        public decimal TotalEstimatedPrice => Services.Sum(x => x.EstimatedPrice);
    }
}
