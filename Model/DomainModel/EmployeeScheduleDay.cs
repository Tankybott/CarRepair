using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DomainModel
{
    public class EmployeeScheduleDay
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(EmployeeProfile))]
        public int EmployeeProfileId { get; set; }
        public EmployeeProfile EmployeeProfile { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public TimeOnly StartDateTime { get; set; }
        public TimeOnly EndDateTime { get; set; }
    }

}
