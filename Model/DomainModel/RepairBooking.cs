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
    public class RepairBooking : ISoftDeletable
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Repair))]
        public int RepairId { get; set; }
        public Repair Repair { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
