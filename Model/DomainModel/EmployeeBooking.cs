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
    public class EmployeeBooking : ISoftDeletable
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(EmployeeProfile))]
        public int EmployeeProfileId { get; set; }
        public EmployeeProfile EmployeeProfile { get; set; }

        public int? TaskId { get; set; }
        public WorkTask? Task { get; set; }

        public BookingType BookingType { get; set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
    public enum BookingType
    {
        Task = 0,
        Vacation = 1,
        Other = 2
    }
}
