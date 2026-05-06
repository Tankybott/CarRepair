namespace Model.DTOs.IntranetDto
{
    public class IntranetPartReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int StatusValue { get; set; }
        public int RepairId { get; set; }
        public string? Manufacturer { get; set; }
        public string? Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
