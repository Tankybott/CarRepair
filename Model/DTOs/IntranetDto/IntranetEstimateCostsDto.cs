namespace Model.DTOs.IntranetDto
{
    public class IntranetEstimateCostsDto
    {
        public int RepairId { get; set; }
        public List<IntranetServiceCostItemDto> ServiceCosts { get; set; } = new();
        public List<IntranetPartEstimateItemDto> Parts { get; set; } = new();
        public List<IntranetOtherCostItemDto> OtherCosts { get; set; } = new();
    }
}
