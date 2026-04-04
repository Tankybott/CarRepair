using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DomainModel
{
    public class WorkTaskComment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(EmployeeProfile))]
        public int EmployeeProfileId { get; set; }
        public EmployeeProfile EmployeeProfile { get; set; }

        [ForeignKey(nameof(WorkTask))]
        public int WorkTaskId { get; set; }
        public WorkTask WorkTask { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
