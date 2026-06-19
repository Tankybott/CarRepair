using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.WebsiteConfigRelated.Interface;

namespace Service.WebsiteConfigRelated
{
    public class WebsiteConfigUpdater : IWebsiteConfigUpdater
    {
        private readonly IWebsiteConfigRepository _repo;

        public WebsiteConfigUpdater(IWebsiteConfigRepository repo)
        {
            _repo = repo;
        }

        public async Task<WebsiteConfigDto> UpdateAsync(WebsiteConfigDto dto)
        {
            var config = await _repo.GetConfigAsync(tracked: true);

            config.PortalHomeTitle = dto.PortalHomeTitle;
            config.PortalHomeText = dto.PortalHomeText;
            config.MondaySchedule = ParseDaySchedule(dto.MondaySchedule);
            config.TuesdaySchedule = ParseDaySchedule(dto.TuesdaySchedule);
            config.WednesdaySchedule = ParseDaySchedule(dto.WednesdaySchedule);
            config.ThursdaySchedule = ParseDaySchedule(dto.ThursdaySchedule);
            config.FridaySchedule = ParseDaySchedule(dto.FridaySchedule);
            config.SaturdaySchedule = ParseDaySchedule(dto.SaturdaySchedule);
            config.SundaySchedule = ParseDaySchedule(dto.SundaySchedule);

            _repo.Update(config);
            await _repo.SaveAsync();

            return WebsiteConfigReader.MapToDto(config);
        }

        private static DaySchedule? ParseDaySchedule(DayScheduleDto? dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.OpenTime) || string.IsNullOrEmpty(dto.CloseTime))
                return null;

            return new DaySchedule
            {
                OpenTime = TimeOnly.Parse(dto.OpenTime),
                CloseTime = TimeOnly.Parse(dto.CloseTime),
            };
        }
    }
}
