using DataAccess.Repository.Interfaces;
using Moq;
using Service.ServiceRelated;
using System.Linq.Expressions;

namespace CarRepairTest.ServiceRelated
{
    public class ServiceDeleterTests
    {
        private readonly Mock<IServiceRepository> _repoMock = new();
        private readonly ServiceDeleter _sut;

        public ServiceDeleterTests()
        {
            _sut = new ServiceDeleter(_repoMock.Object);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenServiceNotFound()
        {
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Model.DomainModel.Service, object>>[]>()))
                .ReturnsAsync((Model.DomainModel.Service?)null);

            await Assert.ThrowsAsync<Exception>(() => _sut.DeleteAsync(99));
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRemove_WhenServiceExists()
        {
            var entity = new Model.DomainModel.Service { Id = 1 };
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Model.DomainModel.Service, object>>[]>()))
                .ReturnsAsync(entity);

            await _sut.DeleteAsync(1);

            _repoMock.Verify(r => r.Remove(entity), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallSaveAsync_WhenServiceExists()
        {
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Model.DomainModel.Service, object>>[]>()))
                .ReturnsAsync(new Model.DomainModel.Service { Id = 1 });

            await _sut.DeleteAsync(1);

            _repoMock.Verify(r => r.SaveAsync(), Times.Once);
        }
    }
}
