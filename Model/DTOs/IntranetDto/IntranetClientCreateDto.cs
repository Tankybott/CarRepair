using System.ComponentModel.DataAnnotations;

namespace Model.DTOs.IntranetDto
{
    public class IntranetClientCreateDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^\+?\d+$", ErrorMessage = "Phone number must contain only digits, optionally starting with +.")]
        public string PhoneNumber { get; set; } = string.Empty;

    }
}
