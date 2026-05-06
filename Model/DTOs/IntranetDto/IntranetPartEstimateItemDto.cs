namespace Model.DTOs.IntranetDto
{
    public class IntranetPartEstimateItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Manufacturer { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
