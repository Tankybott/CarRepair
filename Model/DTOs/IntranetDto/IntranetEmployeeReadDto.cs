namespace Model.DTOs.IntranetDto
{
    public class IntranetEmployeeReadDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EmployeeNumber { get; set; } = string.Empty;
        public List<int> SpecializationIds { get; set; } = new();
        public List<string> SpecializationNames { get; set; } = new();
    }
}
