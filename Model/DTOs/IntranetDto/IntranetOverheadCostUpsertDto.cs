using System.ComponentModel.DataAnnotations;

namespace Model.DTOs.IntranetDto
{
    public class IntranetOverheadCostUpsertDto
    {
        public int RepairId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Cost must be a non-negative number.")]
        public decimal Cost { get; set; }
    }
}
