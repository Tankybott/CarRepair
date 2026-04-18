using Model.DTOs.PortalDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class OfferViewModel
    {
        public IEnumerable<OfferServiceTypeDto> Categories { get; set; } = new List<OfferServiceTypeDto>();
    }
}
