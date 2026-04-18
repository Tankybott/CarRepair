using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs.PortalDto
{
    public class CarAndRepairCarDto
    {
        public int Id { get; set; }

        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;

        public string VIN { get; set; } = string.Empty;

        public int Year { get; set; }
    }
}
