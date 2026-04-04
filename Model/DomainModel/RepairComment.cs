using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DomainModel
{
    public class RepairComment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Repair))]
        public int RepairId { get; set; }
        public Repair Repair { get; set; }

        public string Comment { get; set; } = string.Empty;

        public DateTime? DeletedAt { get; set; }
    }
}
