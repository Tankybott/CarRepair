using Model.DTOs.IntranetDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class IntranetServiceIndexVM
    {
        public IEnumerable<IntranetServiceReadDto> Items { get; set; } = Enumerable.Empty<IntranetServiceReadDto>();
        public IEnumerable<string> ServiceTypes { get; set; } = Enumerable.Empty<string>();
    }
}
