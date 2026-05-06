using Model.DomainModel;
using System.ComponentModel.DataAnnotations;

namespace Model.DTOs.IntranetDto
{
    public class IntranetCarUpsertDto
    {
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public string Brand { get; set; } = string.Empty;

        [Required]
        public string Model { get; set; } = string.Empty;

        [Required]
        public string VIN { get; set; } = string.Empty;

        [Required]
        public string EngineCode { get; set; } = string.Empty;

        [Required]
        [Range(1886, 2100)]
        public int Year { get; set; }

        [Required]
        public FuelType FuelType { get; set; }

        [Required]
        public TransmissionType Transmission { get; set; }

        [Required]
        public BodyType BodyType { get; set; }

        public string? ClientNotes { get; set; }
    }
}
