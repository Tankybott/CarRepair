using Model.DTOs.IntranetDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class IntranetClientViewModel
    {
        public IEnumerable<IntranetClientReadDto> Clients { get; set; } = Enumerable.Empty<IntranetClientReadDto>();
    }
}
