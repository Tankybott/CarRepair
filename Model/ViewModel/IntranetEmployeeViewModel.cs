using Model.DTOs.IntranetDto;

namespace Model.ViewModel
{
    public class IntranetEmployeeViewModel
    {
        public IEnumerable<IntranetEmployeeReadDto> Employees { get; set; } = Enumerable.Empty<IntranetEmployeeReadDto>();
        public IEnumerable<ServiceTypeDto> ServiceTypes { get; set; } = Enumerable.Empty<ServiceTypeDto>();
    }
}
