namespace Model.DTOs.IntranetDto
{
    public class IntranetCostEstimationItemReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public string Type { get; set; } = string.Empty;
        public int TypeValue { get; set; }
    }
}
