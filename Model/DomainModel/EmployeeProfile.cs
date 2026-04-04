using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DomainModel
{
    public class EmployeeProfile
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser? ApplicationUser { get; set; }

        public ICollection<ServiceType> Specializations { get; set; } = new List<ServiceType>();
        public ICollection<EmployeeScheduleDay> EmployeeScheduleDays { get; set; } = new List<EmployeeScheduleDay>();
        public ICollection<WorkTask> AssignedTasks { get; set; } = new List<WorkTask>();
        public ICollection<EmployeeBooking> EmployeeBookings { get; set; } = new List<EmployeeBooking>();

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string EmployeeNumber { get; set; } = string.Empty;
    }
}
