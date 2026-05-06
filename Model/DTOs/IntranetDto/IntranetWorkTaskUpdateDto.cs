namespace Model.DTOs.IntranetDto
{
    public class IntranetWorkTaskUpdateDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime PredictedStart { get; set; }
        public DateTime PredictedEnd { get; set; }
        public List<int> AssignedEmployeeIds { get; set; } = new();
    }
}
