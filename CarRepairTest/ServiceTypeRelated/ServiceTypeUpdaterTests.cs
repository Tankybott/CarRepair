using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.ServiceTypeRelated;
using System.Linq.Expressions;

namespace CarRepairTest.ServiceTypeRelated
{
    public class ServiceTypeUpdaterTests
    {
        private readonly Mock<IServiceTypeRepository> _repoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly ServiceTypeUpdater _sut;

        public ServiceTypeUpdaterTests()
        {
            _sut = new ServiceTypeUpdater(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenServiceTypeNotFound()
        {
            var dto = new ServiceTypeDto { Id = 99 };
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()))
                .ReturnsAsync((ServiceType?)null);

            await Assert.ThrowsAsync<Exception>(() => _sut.UpdateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateNameAndDescription_WhenServiceTypeExists()
        {
            var entity = new ServiceType { Id = 1, Name = "Old", Description = "Old desc" };
            var dto = new ServiceTypeDto { Id = 1, Name = "New", Description = "New desc" };
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()))
                .ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<ServiceTypeDto>(entity)).Returns(new ServiceTypeDto());

            await _sut.UpdateAsync(dto);

            Assert.Equal("New", entity.Name);
            Assert.Equal("New desc", entity.Description);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallUpdate_WhenServiceTypeExists()
        {
            var entity = new ServiceType { Id = 1 };
            var dto = new ServiceTypeDto { Id = 1 };
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()))
                .ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<ServiceTypeDto>(entity)).Returns(new ServiceTypeDto());

            await _sut.UpdateAsync(dto);

            _repoMock.Verify(r => r.Update(entity), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallSaveAsync_WhenServiceTypeExists()
        {
            var entity = new ServiceType { Id = 1 };
            var dto = new ServiceTypeDto { Id = 1 };
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()))
                .ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<ServiceTypeDto>(entity)).Returns(new ServiceTypeDto());

            await _sut.UpdateAsync(dto);

            _repoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnMappedDto_WhenServiceTypeIsUpdated()
        {
            var entity = new ServiceType { Id = 1 };
            var dto = new ServiceTypeDto { Id = 1 };
            var expected = new ServiceTypeDto { Id = 1, Name = "Engine Work" };
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()))
                .ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<ServiceTypeDto>(entity)).Returns(expected);

            var result = await _sut.UpdateAsync(dto);

            Assert.Equal(expected, result);
        }
    }
}
