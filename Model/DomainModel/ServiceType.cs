using Model.DomainModel.intrefaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DomainModel
{
    public class ServiceType: ISoftDeletable
    {
        [Key]
        public int Id { get; set; }

        public ICollection<Service> Services { get; set; } = new List<Service>();
        public ICollection<EmployeeProfile> EmployeeProfiles { get; set; } = new List<EmployeeProfile>();

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public DateTime? DeletedAt { get; set; }
    }
}
