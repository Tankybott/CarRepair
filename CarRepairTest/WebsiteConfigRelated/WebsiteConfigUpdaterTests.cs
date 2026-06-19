using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.WebsiteConfigRelated;

namespace CarRepairTest.WebsiteConfigRelated
{
    public class WebsiteConfigUpdaterTests
    {
        private readonly Mock<IWebsiteConfigRepository> _repoMock = new();
        private readonly WebsiteConfigUpdater _sut;

        public WebsiteConfigUpdaterTests()
        {
            _sut = new WebsiteConfigUpdater(_repoMock.Object);
        }

        private void SetupConfig(WebsiteConfig config)
        {
            _repoMock.Setup(r => r.GetConfigAsync(It.IsAny<bool>())).ReturnsAsync(config);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateTitleAndText_WhenDtoIsValid()
        {
            var config = new WebsiteConfig();
            SetupConfig(config);
            var dto = new WebsiteConfigDto { PortalHomeTitle = "New Title", PortalHomeText = "New Text" };

            await _sut.UpdateAsync(dto);

            Assert.Equal("New Title", config.PortalHomeTitle);
            Assert.Equal("New Text", config.PortalHomeText);
        }

        [Fact]
        public async Task UpdateAsync_ShouldParseDaySchedule_WhenValidScheduleDtoProvided()
        {
            var config = new WebsiteConfig();
            SetupConfig(config);
            var dto = new WebsiteConfigDto
            {
                MondaySchedule = new DayScheduleDto { OpenTime = "08:00", CloseTime = "17:00" }
            };

            await _sut.UpdateAsync(dto);

            Assert.Equal(new TimeOnly(8, 0), config.MondaySchedule!.OpenTime);
            Assert.Equal(new TimeOnly(17, 0), config.MondaySchedule.CloseTime);
        }

        [Fact]
        public async Task UpdateAsync_ShouldSetScheduleToNull_WhenDayScheduleDtoIsNull()
        {
            var config = new WebsiteConfig { MondaySchedule = new DaySchedule { OpenTime = new TimeOnly(8, 0), CloseTime = new TimeOnly(17, 0) } };
            SetupConfig(config);
            var dto = new WebsiteConfigDto { MondaySchedule = null };

            await _sut.UpdateAsync(dto);

            Assert.Null(config.MondaySchedule);
        }

        [Fact]
        public async Task UpdateAsync_ShouldSetScheduleToNull_WhenOpenTimeIsEmpty()
        {
            var config = new WebsiteConfig();
            SetupConfig(config);
            var dto = new WebsiteConfigDto
            {
                MondaySchedule = new DayScheduleDto { OpenTime = "", CloseTime = "17:00" }
            };

            await _sut.UpdateAsync(dto);

            Assert.Null(config.MondaySchedule);
        }

        [Fact]
        public async Task UpdateAsync_ShouldSetScheduleToNull_WhenCloseTimeIsEmpty()
        {
            var config = new WebsiteConfig();
            SetupConfig(config);
            var dto = new WebsiteConfigDto
            {
                MondaySchedule = new DayScheduleDto { OpenTime = "08:00", CloseTime = "" }
            };

            await _sut.UpdateAsync(dto);

            Assert.Null(config.MondaySchedule);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallUpdate_WhenDtoIsValid()
        {
            var config = new WebsiteConfig();
            SetupConfig(config);

            await _sut.UpdateAsync(new WebsiteConfigDto());

            _repoMock.Verify(r => r.Update(config), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallSaveAsync_WhenDtoIsValid()
        {
            SetupConfig(new WebsiteConfig());

            await _sut.UpdateAsync(new WebsiteConfigDto());

            _repoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnDtoReflectingUpdatedConfig_WhenSuccessful()
        {
            var config = new WebsiteConfig();
            SetupConfig(config);
            var dto = new WebsiteConfigDto { PortalHomeTitle = "Shop", PortalHomeText = "Hello" };

            var result = await _sut.UpdateAsync(dto);

            Assert.Equal("Shop", result.PortalHomeTitle);
            Assert.Equal("Hello", result.PortalHomeText);
        }
    }
}
