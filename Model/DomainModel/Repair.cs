using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DomainModel
{
    public class Repair
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Car))]
        public int CarId { get; set; }
        public Car Car { get; set; }

        public RepairBooking Booking { get; set; }

        public ICollection<RepairImage> Images { get; set; } = new List<RepairImage>();
        public ICollection<Service> ServicesSelected { get; set; } = new List<Service>();
        public ICollection<WorkTask> Tasks { get; set; } = new List<WorkTask>();
        public ICollection<RepairComment> ManagerComments { get; set; } = new List<RepairComment>();
        public ICollection<CostEstimationItem> CostEstimationCollection { get; set; } = new List<CostEstimationItem>();
        public ICollection<CostEstimationItem> OverheadCosts { get; set; } = new List<CostEstimationItem>();
        public ICollection<Part> PartsUsed { get; set; } = new List<Part>();   

        public string ClientDescription { get; set; } = string.Empty;

        public decimal PredictedPrice { get; set; }
        public decimal Price { get; set; }

        public RepairStatus Status { get; set; } = RepairStatus.Pending;

        public bool BeingProcessedByManager { get; set; }

        public DateTime? FinishedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public enum RepairStatus
    {
        Pending = 0,
        ManagerReviewed = 1,
        ClientAccepted = 2,
        ClientDeclined = 3,
        Scheduled = 4,
        Done = 5,
        Cancelled = 6,
        Delayed = 7
    }
}
