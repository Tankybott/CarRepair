using Model.DTOs.PortalDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class CartViewModel
    {
        public List<CartServiceDto> Services { get; set; } = new();
        public decimal TotalEstimatedPrice => Services.Sum(x => x.EstimatedPrice);
    }
}
