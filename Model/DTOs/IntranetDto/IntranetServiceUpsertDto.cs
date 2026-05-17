using System.ComponentModel.DataAnnotations;

namespace Model.DTOs.IntranetDto
{
    public class IntranetServiceUpsertDto
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a service type.")]
        public int ServiceTypeId { get; set; }

        [Required]
        public string ShortDescription { get; set; } = string.Empty;

        [Required]
        [MinLength(30, ErrorMessage = "Description must be at least 30 characters long.")]
        public string Description { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Average price must be a positive number.")]
        public decimal AveragePrice { get; set; }
    }
}
