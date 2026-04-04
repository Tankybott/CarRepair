using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DomainModel
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }
        public ClientProfile Client { get; set; }

        public ICollection<Repair> Repairs { get; set; } = new List<Repair>();

        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string VIN { get; set; } = string.Empty;
        public string EngineCode { get; set; } = string.Empty;
        public string EngineCapacity { get; set; } = string.Empty;
        public string? ClientNotes { get; set; }

        public int Year { get; set; }

        public FuelType FuelType { get; set; }
        public TransmissionType Transmission { get; set; }
        public BodyType BodyType { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
    public enum FuelType
    {
        Petrol = 0,
        Diesel = 1,
        Hybrid = 2,
        Electric = 3,
        LPG = 4
    }

    public enum TransmissionType
    {
        Manual = 0,
        Automatic = 1,
    }
    public enum BodyType
    {
        Sedan = 0,
        Hatchback = 1,
        SUV = 2,
        Coupe = 3,
        Van = 6,
        Pickup = 7
    }
}
