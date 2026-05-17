using Model.DTOs.IntranetDto;

namespace Model.ViewModel
{
    public class IntranetRepairOverheadViewModel
    {
        public int RepairId { get; set; }
        public string CarLabel { get; set; } = string.Empty;
        public string ClientFullName { get; set; } = string.Empty;
        public List<IntranetCostEstimationItemReadDto> OverheadCosts { get; set; } = new();
    }
}

