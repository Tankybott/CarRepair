using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DTOs.IntranetDto;
using Moq;
using Service.ServiceRelated;
using System.Linq.Expressions;

namespace CarRepairTest.ServiceRelated
{
    public class ServiceUpdaterTests
    {
        private readonly Mock<IServiceRepository> _repoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly ServiceUpdater _sut;

        public ServiceUpdaterTests()
        {
            _sut = new ServiceUpdater(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenServiceNotFound()
        {
            var dto = new IntranetServiceUpsertDto { Id = 99 };
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Model.DomainModel.Service, object>>[]>()))
                .ReturnsAsync((Model.DomainModel.Service?)null);

            await Assert.ThrowsAsync<Exception>(() => _sut.UpdateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEntityProperties_WhenServiceExists()
        {
            var entity = new Model.DomainModel.Service { Id = 1, ShortDescription = "Old" };
            var dto = new IntranetServiceUpsertDto { Id = 1, ShortDescription = "New", ServiceTypeId = 2, AveragePrice = 99 };
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Model.DomainModel.Service, object>>[]>()))
                .ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<IntranetServiceReadDto>(It.IsAny<object>())).Returns(new IntranetServiceReadDto());

            await _sut.UpdateAsync(dto);

            Assert.Equal("New", entity.ShortDescription);
            Assert.Equal(2, entity.ServiceTypeId);
            Assert.Equal(99m, entity.AveragePrice);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallUpdate_WhenServiceExists()
        {
            var entity = new Model.DomainModel.Service { Id = 1 };
            var dto = new IntranetServiceUpsertDto { Id = 1 };
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Model.DomainModel.Service, object>>[]>()))
                .ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<IntranetServiceReadDto>(It.IsAny<object>())).Returns(new IntranetServiceReadDto());

            await _sut.UpdateAsync(dto);

            _repoMock.Verify(r => r.Update(entity), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallSaveAsync_WhenServiceExists()
        {
            var entity = new Model.DomainModel.Service { Id = 1 };
            var dto = new IntranetServiceUpsertDto { Id = 1 };
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Model.DomainModel.Service, object>>[]>()))
                .ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<IntranetServiceReadDto>(It.IsAny<object>())).Returns(new IntranetServiceReadDto());

            await _sut.UpdateAsync(dto);

            _repoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnMappedReadDto_WhenServiceIsUpdated()
        {
            var entity = new Model.DomainModel.Service { Id = 1 };
            var dto = new IntranetServiceUpsertDto { Id = 1 };
            var expected = new IntranetServiceReadDto { Id = 1, ServiceName = "Brake Check" };
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Model.DomainModel.Service, object>>[]>()))
                .ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<IntranetServiceReadDto>(It.IsAny<object>())).Returns(expected);

            var result = await _sut.UpdateAsync(dto);

            Assert.Equal(expected, result);
        }
    }
}
