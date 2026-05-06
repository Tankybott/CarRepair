using Model.DomainModel.intrefaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DomainModel
{
    public class Part : ISoftDeletable
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Repair))]
        public int RepairId { get; set; }
        public Repair Repair { get; set; }

        public string Name { get; set; } = string.Empty;
        public string? Manufacturer { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string? Description { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; } = 1;

        public PartStatus Status { get; set; } = PartStatus.Pending;

        public DateTime? DeliveredAt { get; set; }
        public DateTime? InstalledAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public enum PartStatus
    {
        Pending = 0,
        Ordered = 1,
        Delivered = 2,
        Installed = 3,
        Cancelled = 4
    }
}
