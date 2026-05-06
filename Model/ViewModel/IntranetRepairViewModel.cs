using Model.DTOs.IntranetDto;

namespace Model.ViewModel
{
    public class IntranetRepairViewModel
    {
        public IEnumerable<IntranetRepairReadDto> Repairs { get; set; } = Enumerable.Empty<IntranetRepairReadDto>();
        public IEnumerable<IntranetClientReadDto> Clients { get; set; } = Enumerable.Empty<IntranetClientReadDto>();
        public IEnumerable<IntranetServiceReadDto> Services { get; set; } = Enumerable.Empty<IntranetServiceReadDto>();
    }
}
