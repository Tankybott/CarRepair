using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.WebsiteConfigRelated.Interface;

namespace Service.WebsiteConfigRelated
{
    public class WebsiteConfigReader : IWebsiteConfigReader
    {
        private readonly IWebsiteConfigRepository _repo;

        public WebsiteConfigReader(IWebsiteConfigRepository repo)
        {
            _repo = repo;
        }

        public async Task<WebsiteConfigDto> GetConfigAsync()
        {
            var config = await _repo.GetConfigAsync();
            return MapToDto(config);
        }

        internal static WebsiteConfigDto MapToDto(WebsiteConfig config) => new()
        {
            PortalHomeTitle = config.PortalHomeTitle,
            PortalHomeText = config.PortalHomeText,
            MondaySchedule = MapDaySchedule(config.MondaySchedule),
            TuesdaySchedule = MapDaySchedule(config.TuesdaySchedule),
            WednesdaySchedule = MapDaySchedule(config.WednesdaySchedule),
            ThursdaySchedule = MapDaySchedule(config.ThursdaySchedule),
            FridaySchedule = MapDaySchedule(config.FridaySchedule),
            SaturdaySchedule = MapDaySchedule(config.SaturdaySchedule),
            SundaySchedule = MapDaySchedule(config.SundaySchedule),
        };

        private static DayScheduleDto? MapDaySchedule(DaySchedule? schedule)
        {
            if (schedule?.OpenTime == null || schedule.CloseTime == null)
                return null;

            return new DayScheduleDto
            {
                OpenTime = schedule.OpenTime.Value.ToString("HH:mm"),
                CloseTime = schedule.CloseTime.Value.ToString("HH:mm"),
            };
        }
    }
}
