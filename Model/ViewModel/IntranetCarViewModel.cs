using Model.DTOs.IntranetDto;

namespace Model.ViewModel
{
    public class IntranetCarViewModel
    {
        public IEnumerable<IntranetCarReadDto> Cars { get; set; } = Enumerable.Empty<IntranetCarReadDto>();
        public IEnumerable<IntranetClientReadDto> Clients { get; set; } = Enumerable.Empty<IntranetClientReadDto>();
    }
}
