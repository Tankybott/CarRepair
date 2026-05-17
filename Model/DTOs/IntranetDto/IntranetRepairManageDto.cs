namespace Model.DTOs.IntranetDto
{
    public class IntranetRepairManageDto
    {
        public int RepairId { get; set; }
        public string Status { get; set; } = string.Empty;
        public int StatusValue { get; set; }
        public string ClientDescription { get; set; } = string.Empty;
        public decimal PredictedPrice { get; set; }

        public int CarId { get; set; }
        public string CarBrand { get; set; } = string.Empty;
        public string CarModel { get; set; } = string.Empty;
        public int CarYear { get; set; }
        public string CarVIN { get; set; } = string.Empty;
        public string CarEngineCode { get; set; } = string.Empty;
        public string CarFuelType { get; set; } = string.Empty;
        public string CarTransmission { get; set; } = string.Empty;
        public string CarBodyType { get; set; } = string.Empty;

        public string ClientName { get; set; } = string.Empty;
        public string ClientSurname { get; set; } = string.Empty;
        public string ClientFullName { get; set; } = string.Empty;
        public string ClientEmail { get; set; } = string.Empty;

        public DateTime? BookingStart { get; set; }
        public DateTime? BookingEnd { get; set; }

        public List<IntranetServiceReadDto> Services { get; set; } = new();
        public List<IntranetPartReadDto> Parts { get; set; } = new();
        public List<IntranetCostEstimationItemReadDto> CostEstimations { get; set; } = new();
        public List<IntranetCostEstimationItemReadDto> OverheadCosts { get; set; } = new();
        public List<IntranetServiceReadDto> AllAvailableServices { get; set; } = new();
    }
}
