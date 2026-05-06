using System.ComponentModel.DataAnnotations;

namespace Model.DTOs.IntranetDto
{
    public class IntranetEmployeeUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;

        [Required]
        public string EmployeeNumber { get; set; } = string.Empty;

        public List<int> SpecializationIds { get; set; } = new();

        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string? NewPassword { get; set; }

        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        public string? ConfirmNewPassword { get; set; }
    }
}
