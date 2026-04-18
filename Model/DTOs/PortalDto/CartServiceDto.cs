using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs.PortalDto
{
    public class CartServiceDto
    {
        public string ServiceName { get; set; }
        public string ShortDescription { get; set; }
        public decimal EstimatedPrice { get; set; }
    }
}
