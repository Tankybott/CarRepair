using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Moq;
using Service.WebsiteConfigRelated;

namespace CarRepairTest.WebsiteConfigRelated
{
    public class WorkingHoursValidatorTests
    {
        private readonly Mock<IWebsiteConfigRepository> _repoMock = new();
        private readonly WorkingHoursValidator _sut;

        // Monday 2025-06-09, Tuesday 2025-06-10, Wednesday 2025-06-11
        private static readonly DateTime Monday = new(2025, 6, 9);
        private static readonly DateTime Tuesday = new(2025, 6, 10);
        private static readonly DateTime Wednesday = new(2025, 6, 11);

        public WorkingHoursValidatorTests()
        {
            _sut = new WorkingHoursValidator(_repoMock.Object);
        }

        private static DaySchedule OpenSchedule(int openHour = 8, int closeHour = 17) => new()
        {
            OpenTime = new TimeOnly(openHour, 0),
            CloseTime = new TimeOnly(closeHour, 0)
        };

        private static WebsiteConfig FullWeekOpenConfig() => new()
        {
            MondaySchedule = OpenSchedule(),
            TuesdaySchedule = OpenSchedule(),
            WednesdaySchedule = OpenSchedule(),
            ThursdaySchedule = OpenSchedule(),
            FridaySchedule = OpenSchedule(),
            SaturdaySchedule = null,
            SundaySchedule = null
        };

        private void SetupConfig(WebsiteConfig config)
        {
            _repoMock.Setup(r => r.GetConfigAsync(It.IsAny<bool>())).ReturnsAsync(config);
        }

        [Fact]
        public async Task ValidateAsync_ShouldThrow_WhenStartDayHasNoSchedule()
        {
            var config = FullWeekOpenConfig();
            config.MondaySchedule = null;
            SetupConfig(config);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _sut.ValidateAsync(Monday.AddHours(9), Monday.AddHours(15)));
        }

        [Fact]
        public async Task ValidateAsync_ShouldThrow_WhenStartTimeIsBeforeOpeningTime()
        {
            SetupConfig(FullWeekOpenConfig());

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _sut.ValidateAsync(Monday.AddHours(7), Monday.AddHours(15)));
        }

        [Fact]
        public async Task ValidateAsync_ShouldNotThrow_WhenStartTimeEqualsOpeningTime()
        {
            SetupConfig(FullWeekOpenConfig());

            var exception = await Record.ExceptionAsync(
                () => _sut.ValidateAsync(Monday.AddHours(8), Monday.AddHours(15)));

            Assert.Null(exception);
        }

        [Fact]
        public async Task ValidateAsync_ShouldThrow_WhenEndDayHasNoSchedule()
        {
            var config = FullWeekOpenConfig();
            config.TuesdaySchedule = null;
            SetupConfig(config);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _sut.ValidateAsync(Monday.AddHours(9), Tuesday.AddHours(10)));
        }

        [Fact]
        public async Task ValidateAsync_ShouldThrow_WhenEndTimeIsAfterClosingTime()
        {
            SetupConfig(FullWeekOpenConfig());

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _sut.ValidateAsync(Monday.AddHours(9), Monday.AddHours(18)));
        }

        [Fact]
        public async Task ValidateAsync_ShouldNotThrow_WhenEndTimeEqualsClosingTime()
        {
            SetupConfig(FullWeekOpenConfig());

            var exception = await Record.ExceptionAsync(
                () => _sut.ValidateAsync(Monday.AddHours(9), Monday.AddHours(17)));

            Assert.Null(exception);
        }

        [Fact]
        public async Task ValidateAsync_ShouldThrow_WhenTaskSpansClosedIntermediateDay()
        {
            var config = FullWeekOpenConfig();
            config.TuesdaySchedule = null;
            SetupConfig(config);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _sut.ValidateAsync(Monday.AddHours(9), Wednesday.AddHours(15)));
        }

        [Fact]
        public async Task ValidateAsync_ShouldNotThrow_WhenConsecutiveDaysAndNoIntermediateDayToCheck()
        {
            SetupConfig(FullWeekOpenConfig());

            var exception = await Record.ExceptionAsync(
                () => _sut.ValidateAsync(Monday.AddHours(9), Tuesday.AddHours(15)));

            Assert.Null(exception);
        }

        [Fact]
        public async Task ValidateAsync_ShouldNotThrow_WhenTaskIsWithinWorkingHoursOnSingleDay()
        {
            SetupConfig(FullWeekOpenConfig());

            var exception = await Record.ExceptionAsync(
                () => _sut.ValidateAsync(Monday.AddHours(9), Monday.AddHours(16)));

            Assert.Null(exception);
        }

        [Fact]
        public async Task ValidateAsync_ShouldNotThrow_WhenTaskSpansMultipleOpenDays()
        {
            SetupConfig(FullWeekOpenConfig());

            var exception = await Record.ExceptionAsync(
                () => _sut.ValidateAsync(Monday.AddHours(9), Wednesday.AddHours(15)));

            Assert.Null(exception);
        }

        [Fact]
        public async Task ValidateAsync_ShouldThrow_WhenStartDayScheduleHasNullOpenTime()
        {
            var config = FullWeekOpenConfig();
            config.MondaySchedule = new DaySchedule { OpenTime = null, CloseTime = new TimeOnly(17, 0) };
            SetupConfig(config);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _sut.ValidateAsync(Monday.AddHours(9), Monday.AddHours(15)));
        }

        [Fact]
        public async Task ValidateAsync_ShouldThrow_WhenStartDayScheduleHasNullCloseTime()
        {
            var config = FullWeekOpenConfig();
            config.MondaySchedule = new DaySchedule { OpenTime = new TimeOnly(8, 0), CloseTime = null };
            SetupConfig(config);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _sut.ValidateAsync(Monday.AddHours(9), Monday.AddHours(15)));
        }
    }
}
