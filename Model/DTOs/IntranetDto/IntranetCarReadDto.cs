namespace Model.DTOs.IntranetDto
{
    public class IntranetCarReadDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string OwnerFullName { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string VIN { get; set; } = string.Empty;
        public string EngineCode { get; set; } = string.Empty;
        public int Year { get; set; }
        public string FuelType { get; set; } = string.Empty;
        public int FuelTypeValue { get; set; }
        public string Transmission { get; set; } = string.Empty;
        public int TransmissionValue { get; set; }
        public string BodyType { get; set; } = string.Empty;
        public int BodyTypeValue { get; set; }
        public string? ClientNotes { get; set; }
    }
}
