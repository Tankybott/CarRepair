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
    public class CostEstimationItem : ISoftDeletable
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Repair))]
        public int RepairId { get; set; }
        public Repair Repair { get; set; }

        public CostEstimationItemType Type { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal Cost { get; set; }

        public DateTime? DeletedAt { get; set; }
    }

    public enum CostEstimationItemType
    {
        Service = 0,
        Part = 1,
        Other = 2,
        Overhead = 3
    }
}
