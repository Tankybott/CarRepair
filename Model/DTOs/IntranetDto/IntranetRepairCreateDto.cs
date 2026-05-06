using System.ComponentModel.DataAnnotations;

namespace Model.DTOs.IntranetDto
{
    public class IntranetRepairCreateDto
    {
        [Required]
        public int CarId { get; set; }

        public List<int> ServiceIds { get; set; } = new();

        [Required]
        public string ClientDescription { get; set; } = string.Empty;
    }
}
