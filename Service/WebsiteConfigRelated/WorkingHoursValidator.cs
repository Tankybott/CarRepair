using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Service.WebsiteConfigRelated.Interface;

namespace Service.WebsiteConfigRelated
{
    public class WorkingHoursValidator : IWorkingHoursValidator
    {
        private readonly IWebsiteConfigRepository _repo;

        public WorkingHoursValidator(IWebsiteConfigRepository repo)
        {
            _repo = repo;
        }

        public async Task ValidateAsync(DateTime start, DateTime end)
        {
            var config = await _repo.GetConfigAsync();

            var startSchedule = GetScheduleForDay(config, start.DayOfWeek);
            if (startSchedule == null)
                throw new InvalidOperationException($"Workshop is closed on {start.DayOfWeek}s.");

            var startTime = TimeOnly.FromDateTime(start);
            if (startTime < startSchedule.OpenTime!.Value)
                throw new InvalidOperationException(
                    $"Task start ({startTime.ToString("HH:mm")}) is before workshop opening time ({startSchedule.OpenTime.Value.ToString("HH:mm")}) on {start.DayOfWeek}.");

            var endSchedule = GetScheduleForDay(config, end.DayOfWeek);
            if (endSchedule == null)
                throw new InvalidOperationException($"Workshop is closed on {end.DayOfWeek}s.");

            var endTime = TimeOnly.FromDateTime(end);
            if (endTime > endSchedule.CloseTime!.Value)
                throw new InvalidOperationException(
                    $"Task end ({endTime.ToString("HH:mm")}) is after workshop closing time ({endSchedule.CloseTime.Value.ToString("HH:mm")}) on {end.DayOfWeek}.");

            for (var day = start.Date.AddDays(1); day < end.Date; day = day.AddDays(1))
            {
                if (GetScheduleForDay(config, day.DayOfWeek) == null)
                    throw new InvalidOperationException(
                        $"Task spans {day:yyyy-MM-dd} ({day.DayOfWeek}), but the workshop is closed that day.");
            }
        }

        private static DaySchedule? GetScheduleForDay(WebsiteConfig config, DayOfWeek day)
        {
            var s = day switch
            {
                DayOfWeek.Monday => config.MondaySchedule,
                DayOfWeek.Tuesday => config.TuesdaySchedule,
                DayOfWeek.Wednesday => config.WednesdaySchedule,
                DayOfWeek.Thursday => config.ThursdaySchedule,
                DayOfWeek.Friday => config.FridaySchedule,
                DayOfWeek.Saturday => config.SaturdaySchedule,
                DayOfWeek.Sunday => config.SundaySchedule,
                _ => null
            };
            return s?.OpenTime != null && s.CloseTime != null ? s : null;
        }
    }
}
