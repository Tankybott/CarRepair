namespace Model.DTOs.IntranetDto
{
    public class IntranetWorkTaskCreateDto
    {
        public int RepairId { get; set; }
        public int? ServiceId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime PredictedStart { get; set; }
        public DateTime PredictedEnd { get; set; }
        public List<int> AssignedEmployeeIds { get; set; } = new();
    }
}
