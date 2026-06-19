using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.WebsiteConfigRelated;

namespace CarRepairTest.WebsiteConfigRelated
{
    public class WebsiteConfigReaderTests
    {
        private readonly Mock<IWebsiteConfigRepository> _repoMock = new();
        private readonly WebsiteConfigReader _sut;

        public WebsiteConfigReaderTests()
        {
            _sut = new WebsiteConfigReader(_repoMock.Object);
        }

        private static WebsiteConfig BuildConfig(
            string title = "Title",
            string text = "Text",
            DaySchedule? monday = null) => new()
        {
            PortalHomeTitle = title,
            PortalHomeText = text,
            MondaySchedule = monday
        };

        private static DaySchedule OpenSchedule(int open = 8, int close = 17) => new()
        {
            OpenTime = new TimeOnly(open, 0),
            CloseTime = new TimeOnly(close, 0)
        };

        [Fact]
        public async Task GetConfigAsync_ShouldReturnDtoWithCorrectTitle_WhenConfigHasTitle()
        {
            _repoMock.Setup(r => r.GetConfigAsync(It.IsAny<bool>())).ReturnsAsync(BuildConfig(title: "My Shop"));

            var result = await _sut.GetConfigAsync();

            Assert.Equal("My Shop", result.PortalHomeTitle);
        }

        [Fact]
        public async Task GetConfigAsync_ShouldReturnDtoWithCorrectText_WhenConfigHasText()
        {
            _repoMock.Setup(r => r.GetConfigAsync(It.IsAny<bool>())).ReturnsAsync(BuildConfig(text: "Welcome text"));

            var result = await _sut.GetConfigAsync();

            Assert.Equal("Welcome text", result.PortalHomeText);
        }

        [Fact]
        public async Task GetConfigAsync_ShouldReturnNullForDaySchedule_WhenScheduleIsNull()
        {
            _repoMock.Setup(r => r.GetConfigAsync(It.IsAny<bool>())).ReturnsAsync(BuildConfig(monday: null));

            var result = await _sut.GetConfigAsync();

            Assert.Null(result.MondaySchedule);
        }

        [Fact]
        public async Task GetConfigAsync_ShouldReturnNullForDaySchedule_WhenOpenTimeIsNull()
        {
            var schedule = new DaySchedule { OpenTime = null, CloseTime = new TimeOnly(17, 0) };
            _repoMock.Setup(r => r.GetConfigAsync(It.IsAny<bool>())).ReturnsAsync(BuildConfig(monday: schedule));

            var result = await _sut.GetConfigAsync();

            Assert.Null(result.MondaySchedule);
        }

        [Fact]
        public async Task GetConfigAsync_ShouldReturnNullForDaySchedule_WhenCloseTimeIsNull()
        {
            var schedule = new DaySchedule { OpenTime = new TimeOnly(8, 0), CloseTime = null };
            _repoMock.Setup(r => r.GetConfigAsync(It.IsAny<bool>())).ReturnsAsync(BuildConfig(monday: schedule));

            var result = await _sut.GetConfigAsync();

            Assert.Null(result.MondaySchedule);
        }

        [Fact]
        public async Task GetConfigAsync_ShouldReturnDayScheduleDto_WhenBothTimesAreSet()
        {
            _repoMock.Setup(r => r.GetConfigAsync(It.IsAny<bool>())).ReturnsAsync(BuildConfig(monday: OpenSchedule()));

            var result = await _sut.GetConfigAsync();

            Assert.NotNull(result.MondaySchedule);
        }

        [Fact]
        public async Task GetConfigAsync_ShouldReturnParsableOpenTime_WhenScheduleIsValid()
        {
            _repoMock.Setup(r => r.GetConfigAsync(It.IsAny<bool>())).ReturnsAsync(BuildConfig(monday: OpenSchedule(open: 8, close: 17)));

            var result = await _sut.GetConfigAsync();

            Assert.Equal(new TimeOnly(8, 0), TimeOnly.Parse(result.MondaySchedule!.OpenTime));
        }

        [Fact]
        public async Task GetConfigAsync_ShouldReturnParsableCloseTime_WhenScheduleIsValid()
        {
            _repoMock.Setup(r => r.GetConfigAsync(It.IsAny<bool>())).ReturnsAsync(BuildConfig(monday: OpenSchedule(open: 8, close: 17)));

            var result = await _sut.GetConfigAsync();

            Assert.Equal(new TimeOnly(17, 0), TimeOnly.Parse(result.MondaySchedule!.CloseTime));
        }
    }
}
