using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.PartRelated;
using System.Linq.Expressions;

namespace CarRepairTest.PartRelated
{
    public class PartUpdaterTests
    {
        private readonly Mock<IPartRepository> _partRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly PartUpdater _sut;

        public PartUpdaterTests()
        {
            _sut = new PartUpdater(_partRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenPartNotFound()
        {
            var dto = new IntranetPartUpsertDto { Id = 99 };
            _partRepositoryMock
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Part, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Part, object>>[]>()))
                .ReturnsAsync((Part?)null);

            await Assert.ThrowsAsync<Exception>(() => _sut.UpdateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntityProperties_WhenPartExists()
        {
            var part = new Part { Id = 1, Name = "Old Name" };
            var dto = new IntranetPartUpsertDto { Id = 1, Name = "New Name" };
            _partRepositoryMock
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Part, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Part, object>>[]>()))
                .ReturnsAsync(part);
            _mapperMock.Setup(m => m.Map<IntranetPartReadDto>(part)).Returns(new IntranetPartReadDto());

            await _sut.UpdateAsync(dto);

            Assert.Equal("New Name", part.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallUpdate_WhenPartExists()
        {
            var part = new Part { Id = 1 };
            var dto = new IntranetPartUpsertDto { Id = 1 };
            _partRepositoryMock
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Part, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Part, object>>[]>()))
                .ReturnsAsync(part);
            _mapperMock.Setup(m => m.Map<IntranetPartReadDto>(part)).Returns(new IntranetPartReadDto());

            await _sut.UpdateAsync(dto);

            _partRepositoryMock.Verify(r => r.Update(part), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallSaveAsync_WhenPartExists()
        {
            var part = new Part { Id = 1 };
            var dto = new IntranetPartUpsertDto { Id = 1 };
            _partRepositoryMock
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Part, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Part, object>>[]>()))
                .ReturnsAsync(part);
            _mapperMock.Setup(m => m.Map<IntranetPartReadDto>(part)).Returns(new IntranetPartReadDto());

            await _sut.UpdateAsync(dto);

            _partRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnMappedReadDto_WhenPartIsUpdated()
        {
            var part = new Part { Id = 1 };
            var dto = new IntranetPartUpsertDto { Id = 1 };
            var expected = new IntranetPartReadDto { Id = 1, Name = "Brake Pad" };
            _partRepositoryMock
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Part, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Part, object>>[]>()))
                .ReturnsAsync(part);
            _mapperMock.Setup(m => m.Map<IntranetPartReadDto>(part)).Returns(expected);

            var result = await _sut.UpdateAsync(dto);

            Assert.Equal(expected, result);
        }
    }
}
