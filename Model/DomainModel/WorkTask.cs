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
    public class WorkTask : ISoftDeletable
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Repair))]
        public int RepairId { get; set; }
        public Repair Repair { get; set; }

        [ForeignKey(nameof(Service))]
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }

        public ICollection<EmployeeProfile> EmployeesAssigned { get; set; } = new List<EmployeeProfile>();
        public string Description { get; set; } = string.Empty;
        public string? FailDescription { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.Assigned;

        public DateTime PredictedStart { get; set; }
        public DateTime PredicetedEnd { get; set; }
        public DateTime? ActualStart { get; set; }
        public DateTime? ActualEnd { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public enum TaskStatus
    {
        Assigned = 0,
        Completed = 1,
        Cancelled = 2,
        Failed = 3
    }
}
