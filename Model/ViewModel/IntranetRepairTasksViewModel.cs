using Model.DTOs.IntranetDto;

namespace Model.ViewModel
{
    public class IntranetRepairTasksViewModel
    {
        public int RepairId { get; set; }
        public string CarLabel { get; set; } = string.Empty;
        public string ClientFullName { get; set; } = string.Empty;
        public IEnumerable<IntranetWorkTaskReadDto> Tasks { get; set; } = new List<IntranetWorkTaskReadDto>();
        public IEnumerable<IntranetServiceReadDto> Services { get; set; } = new List<IntranetServiceReadDto>();
        public DateTime? ExistingDelivery { get; set; }
        public DateTime? ExistingPickup { get; set; }
    }
}
