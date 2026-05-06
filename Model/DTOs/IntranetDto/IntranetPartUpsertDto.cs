using System.ComponentModel.DataAnnotations;

namespace Model.DTOs.IntranetDto
{
    public class IntranetPartUpsertDto
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please provide a valid Repair ID.")]
        public int RepairId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string SerialNumber { get; set; } = string.Empty;

        public string? Manufacturer { get; set; }

        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Unit price must be a positive number.")]
        public decimal UnitPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; } = 1;
    }
}
