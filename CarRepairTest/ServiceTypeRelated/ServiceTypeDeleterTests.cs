using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Moq;
using Service.ServiceTypeRelated;
using System.Linq.Expressions;

namespace CarRepairTest.ServiceTypeRelated
{
    public class ServiceTypeDeleterTests
    {
        private readonly Mock<IServiceTypeRepository> _serviceTypeRepoMock = new();
        private readonly Mock<IServiceRepository> _serviceRepoMock = new();
        private readonly ServiceTypeDeleter _sut;

        public ServiceTypeDeleterTests()
        {
            _sut = new ServiceTypeDeleter(_serviceTypeRepoMock.Object, _serviceRepoMock.Object);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenServiceTypeNotFound()
        {
            _serviceTypeRepoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()))
                .ReturnsAsync((ServiceType?)null);

            await Assert.ThrowsAsync<Exception>(() => _sut.DeleteAsync(99));
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowInvalidOperationException_WhenActiveServicesExist()
        {
            _serviceTypeRepoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()))
                .ReturnsAsync(new ServiceType { Id = 1 });
            _serviceRepoMock
                .Setup(r => r.Any(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>()))
                .Returns(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.DeleteAsync(1));
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRemove_WhenNoActiveServicesExist()
        {
            var entity = new ServiceType { Id = 1 };
            _serviceTypeRepoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()))
                .ReturnsAsync(entity);
            _serviceRepoMock
                .Setup(r => r.Any(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>()))
                .Returns(false);

            await _sut.DeleteAsync(1);

            _serviceTypeRepoMock.Verify(r => r.Remove(entity), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallSaveAsync_WhenNoActiveServicesExist()
        {
            _serviceTypeRepoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()))
                .ReturnsAsync(new ServiceType { Id = 1 });
            _serviceRepoMock
                .Setup(r => r.Any(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>()))
                .Returns(false);

            await _sut.DeleteAsync(1);

            _serviceTypeRepoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotCallRemove_WhenActiveServicesExist()
        {
            _serviceTypeRepoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()))
                .ReturnsAsync(new ServiceType { Id = 1 });
            _serviceRepoMock
                .Setup(r => r.Any(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>()))
                .Returns(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.DeleteAsync(1));

            _serviceTypeRepoMock.Verify(r => r.Remove(It.IsAny<ServiceType>()), Times.Never);
        }
    }
}
