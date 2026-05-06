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
    public class Repair : ISoftDeletable
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Car))]
        public int CarId { get; set; }
        public Car Car { get; set; }

        public RepairBooking Booking { get; set; }

        public ICollection<Service> ServicesSelected { get; set; } = new List<Service>();
        public ICollection<WorkTask> Tasks { get; set; } = new List<WorkTask>();
        public ICollection<CostEstimationItem> CostEstimationCollection { get; set; } = new List<CostEstimationItem>();
        public ICollection<Part> PartsUsed { get; set; } = new List<Part>();   

        public string ClientDescription { get; set; } = string.Empty;

        public decimal PredictedPrice { get; set; }
        public decimal FinalPrice { get; set; }

        public RepairStatus Status { get; set; } = RepairStatus.Pending;

        public DateTime? FinishedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public enum RepairStatus
    {
        Pending = 0,
        ManagerReviewed = 1,
        ClientAccepted = 2,
        Scheduled = 3,
        Done = 4,
        Cancelled = 5,
    }
}
