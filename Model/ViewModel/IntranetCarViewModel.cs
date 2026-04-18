using Model.DTOs.IntranetDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class IntranetCarViewModel
    {
        public IEnumerable<IntranetCarReadDto> Cars { get; set; } = Enumerable.Empty<IntranetCarReadDto>();
    }
}
