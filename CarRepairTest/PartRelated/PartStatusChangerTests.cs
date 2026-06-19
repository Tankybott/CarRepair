using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.PartRelated;
using System.Linq.Expressions;

namespace CarRepairTest.PartRelated
{
    public class PartStatusChangerTests
    {
        private readonly Mock<IPartRepository> _partRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly PartStatusChanger _sut;

        public PartStatusChangerTests()
        {
            _sut = new PartStatusChanger(_partRepositoryMock.Object, _mapperMock.Object);
        }

        private void SetupPart(Part? part)
        {
            _partRepositoryMock
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Part, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Part, object>>[]>()))
                .ReturnsAsync(part);
        }

        [Fact]
        public async Task ChangeStatusAsync_ShouldThrowException_WhenPartNotFound()
        {
            SetupPart(null);

            await Assert.ThrowsAsync<Exception>(() => _sut.ChangeStatusAsync(99, PartStatus.Ordered));
        }

        [Fact]
        public async Task ChangeStatusAsync_ShouldUpdateStatus_WhenPartExists()
        {
            var part = new Part { Id = 1, Status = PartStatus.Pending };
            SetupPart(part);
            _mapperMock.Setup(m => m.Map<IntranetPartReadDto>(part)).Returns(new IntranetPartReadDto());

            await _sut.ChangeStatusAsync(1, PartStatus.Ordered);

            Assert.Equal(PartStatus.Ordered, part.Status);
        }

        [Fact]
        public async Task ChangeStatusAsync_ShouldSetDeliveredAt_WhenStatusIsDeliveredAndDeliveredAtIsNull()
        {
            var part = new Part { Id = 1, DeliveredAt = null };
            SetupPart(part);
            _mapperMock.Setup(m => m.Map<IntranetPartReadDto>(part)).Returns(new IntranetPartReadDto());

            await _sut.ChangeStatusAsync(1, PartStatus.Delivered);

            Assert.NotNull(part.DeliveredAt);
        }

        [Fact]
        public async Task ChangeStatusAsync_ShouldNotOverwriteDeliveredAt_WhenDeliveredAtIsAlreadySet()
        {
            var originalDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var part = new Part { Id = 1, DeliveredAt = originalDate };
            SetupPart(part);
            _mapperMock.Setup(m => m.Map<IntranetPartReadDto>(part)).Returns(new IntranetPartReadDto());

            await _sut.ChangeStatusAsync(1, PartStatus.Delivered);

            Assert.Equal(originalDate, part.DeliveredAt);
        }

        [Fact]
        public async Task ChangeStatusAsync_ShouldSetInstalledAt_WhenStatusIsInstalledAndInstalledAtIsNull()
        {
            var part = new Part { Id = 1, InstalledAt = null };
            SetupPart(part);
            _mapperMock.Setup(m => m.Map<IntranetPartReadDto>(part)).Returns(new IntranetPartReadDto());

            await _sut.ChangeStatusAsync(1, PartStatus.Installed);

            Assert.NotNull(part.InstalledAt);
        }

        [Fact]
        public async Task ChangeStatusAsync_ShouldNotOverwriteInstalledAt_WhenInstalledAtIsAlreadySet()
        {
            var originalDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var part = new Part { Id = 1, InstalledAt = originalDate };
            SetupPart(part);
            _mapperMock.Setup(m => m.Map<IntranetPartReadDto>(part)).Returns(new IntranetPartReadDto());

            await _sut.ChangeStatusAsync(1, PartStatus.Installed);

            Assert.Equal(originalDate, part.InstalledAt);
        }

        [Fact]
        public async Task ChangeStatusAsync_ShouldCallUpdate_WhenPartExists()
        {
            var part = new Part { Id = 1 };
            SetupPart(part);
            _mapperMock.Setup(m => m.Map<IntranetPartReadDto>(part)).Returns(new IntranetPartReadDto());

            await _sut.ChangeStatusAsync(1, PartStatus.Ordered);

            _partRepositoryMock.Verify(r => r.Update(part), Times.Once);
        }

        [Fact]
        public async Task ChangeStatusAsync_ShouldCallSaveAsync_WhenPartExists()
        {
            var part = new Part { Id = 1 };
            SetupPart(part);
            _mapperMock.Setup(m => m.Map<IntranetPartReadDto>(part)).Returns(new IntranetPartReadDto());

            await _sut.ChangeStatusAsync(1, PartStatus.Ordered);

            _partRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task ChangeStatusAsync_ShouldReturnMappedReadDto_WhenStatusIsChanged()
        {
            var part = new Part { Id = 1 };
            var expected = new IntranetPartReadDto { Id = 1 };
            SetupPart(part);
            _mapperMock.Setup(m => m.Map<IntranetPartReadDto>(part)).Returns(expected);

            var result = await _sut.ChangeStatusAsync(1, PartStatus.Ordered);

            Assert.Equal(expected, result);
        }
    }
}
