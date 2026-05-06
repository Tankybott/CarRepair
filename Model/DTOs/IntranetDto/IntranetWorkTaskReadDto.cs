namespace Model.DTOs.IntranetDto
{
    public class IntranetWorkTaskReadDto
    {
        public int Id { get; set; }
        public int RepairId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int StatusValue { get; set; }
        public DateTime PredictedStart { get; set; }
        public DateTime PredictedEnd { get; set; }
        public int? ServiceId { get; set; }
        public List<string> AssignedEmployeeNames { get; set; } = new();
        public List<int> AssignedEmployeeIds { get; set; } = new();
    }
}
