using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Moq;
using Service.PartRelated;
using System.Linq.Expressions;

namespace CarRepairTest.PartRelated
{
    public class PartDeleterTests
    {
        private readonly Mock<IPartRepository> _partRepositoryMock = new();
        private readonly PartDeleter _sut;

        public PartDeleterTests()
        {
            _sut = new PartDeleter(_partRepositoryMock.Object);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenPartNotFound()
        {
            _partRepositoryMock
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Part, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Part, object>>[]>()))
                .ReturnsAsync((Part?)null);

            await Assert.ThrowsAsync<Exception>(() => _sut.DeleteAsync(99));
        }

        [Fact]
        public async Task DeleteAsync_ShouldSetDeletedAt_WhenPartExists()
        {
            var part = new Part { Id = 1 };
            _partRepositoryMock
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Part, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Part, object>>[]>()))
                .ReturnsAsync(part);

            await _sut.DeleteAsync(1);

            Assert.NotNull(part.DeletedAt);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallSaveAsync_WhenPartExists()
        {
            _partRepositoryMock
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Part, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Part, object>>[]>()))
                .ReturnsAsync(new Part { Id = 1 });

            await _sut.DeleteAsync(1);

            _partRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
        }
    }
}
