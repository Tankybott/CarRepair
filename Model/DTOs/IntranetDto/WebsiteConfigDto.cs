using System.ComponentModel.DataAnnotations;

namespace Model.DTOs.IntranetDto
{
    public class WebsiteConfigDto
    {
        [Required]
        public string PortalHomeTitle { get; set; } = string.Empty;

        [Required]
        public string PortalHomeText { get; set; } = string.Empty;

        public DayScheduleDto? MondaySchedule { get; set; }
        public DayScheduleDto? TuesdaySchedule { get; set; }
        public DayScheduleDto? WednesdaySchedule { get; set; }
        public DayScheduleDto? ThursdaySchedule { get; set; }
        public DayScheduleDto? FridaySchedule { get; set; }
        public DayScheduleDto? SaturdaySchedule { get; set; }
        public DayScheduleDto? SundaySchedule { get; set; }
    }
}
