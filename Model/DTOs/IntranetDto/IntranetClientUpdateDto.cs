using System.ComponentModel.DataAnnotations;

namespace Model.DTOs.IntranetDto
{
    public class IntranetClientUpdateDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\+?\d+$", ErrorMessage = "Phone number must contain only digits, optionally starting with +.")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
