namespace Model.DomainModel
{
    public class WebsiteConfig
    {
        public int Id { get; set; }
        public string PortalHomeTitle { get; set; } = string.Empty;
        public string PortalHomeText { get; set; } = string.Empty;

        public DaySchedule? MondaySchedule { get; set; }
        public DaySchedule? TuesdaySchedule { get; set; }
        public DaySchedule? WednesdaySchedule { get; set; }
        public DaySchedule? ThursdaySchedule { get; set; }
        public DaySchedule? FridaySchedule { get; set; }
        public DaySchedule? SaturdaySchedule { get; set; }
        public DaySchedule? SundaySchedule { get; set; }
    }
}
