using System.ComponentModel.DataAnnotations;

namespace Model.DTOs.PortalDto
{
    public class PortalLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
