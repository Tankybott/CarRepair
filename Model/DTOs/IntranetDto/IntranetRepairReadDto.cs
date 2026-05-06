namespace Model.DTOs.IntranetDto
{
    public class IntranetRepairReadDto
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string CarLabel { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string ClientSurname { get; set; } = string.Empty;
        public string ClientFullName { get; set; } = string.Empty;
        public string ClientDescription { get; set; } = string.Empty;
        public decimal PredictedPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<string> ServiceNames { get; set; } = new();
    }
}
