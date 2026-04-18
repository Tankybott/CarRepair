using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs.IntranetDto
{
    public class ServiceTypeDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MinLength(2, ErrorMessage = "Description must be at least 2 characters long.")]
        public string Description { get; set; } = string.Empty;
    }
}
