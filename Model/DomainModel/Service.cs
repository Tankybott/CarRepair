using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DomainModel
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(ServiceType))]
        public int ServiceTypeId { get; set; }
        public ServiceType ServiceType { get; set; }

        public ICollection<Repair> Repairs { get; set; } = new List<Repair>();

        public string Description { get; set; } = string.Empty;

        public decimal AveragePrice { get; set; }

        public DateTime? DeletedAt { get; set; }
    }

}
