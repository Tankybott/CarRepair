using Model.DTOs.IntranetDto;

namespace Model.ViewModel
{
    public class IntranetPartIndexVM
    {
        public IEnumerable<IntranetPartReadDto> Items { get; set; } = Enumerable.Empty<IntranetPartReadDto>();
    }
}
