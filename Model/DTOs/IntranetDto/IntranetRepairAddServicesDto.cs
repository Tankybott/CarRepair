namespace Model.DTOs.IntranetDto
{
    public class IntranetRepairAddServicesDto
    {
        public int RepairId { get; set; }
        public List<int> ServiceIds { get; set; } = new();
    }
}
