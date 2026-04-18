using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs.PortalDto
{
    public class OfferServiceTypeDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public IEnumerable<OfferServiceDto> Services { get; set; } = new List<OfferServiceDto>();
    }
}
