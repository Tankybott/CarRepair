using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.CarRelated;
using System.Linq.Expressions;

namespace CarRepairTest.CarRelated
{
    public class CarUpdaterTests
    {
        private readonly Mock<ICarRepository> _carRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly CarUpdater _sut;

        public CarUpdaterTests()
        {
            _sut = new CarUpdater(_carRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenCarNotFound()
        {
            var dto = new IntranetCarUpsertDto { Id = 99 };
            _carRepositoryMock
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Car, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Car, object>>[]>()))
                .ReturnsAsync((Car?)null);

            await Assert.ThrowsAsync<Exception>(() => _sut.UpdateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallUpdate_WhenCarExists()
        {
            var car = new Car { Id = 1 };
            var dto = new IntranetCarUpsertDto { Id = 1 };
            _carRepositoryMock
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Car, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Car, object>>[]>()))
                .ReturnsAsync(car);
            _mapperMock.Setup(m => m.Map<IntranetCarReadDto>(It.IsAny<object>())).Returns(new IntranetCarReadDto());

            await _sut.UpdateAsync(dto);

            _carRepositoryMock.Verify(r => r.Update(car), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallSaveAsync_WhenCarExists()
        {
            var car = new Car { Id = 1 };
            var dto = new IntranetCarUpsertDto { Id = 1 };
            _carRepositoryMock
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Car, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Car, object>>[]>()))
                .ReturnsAsync(car);
            _mapperMock.Setup(m => m.Map<IntranetCarReadDto>(It.IsAny<object>())).Returns(new IntranetCarReadDto());

            await _sut.UpdateAsync(dto);

            _carRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnMappedReadDto_WhenCarIsUpdated()
        {
            var car = new Car { Id = 1 };
            var dto = new IntranetCarUpsertDto { Id = 1 };
            var expected = new IntranetCarReadDto { Id = 1, Brand = "BMW" };
            _carRepositoryMock
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Car, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Car, object>>[]>()))
                .ReturnsAsync(car);
            _mapperMock.Setup(m => m.Map<IntranetCarReadDto>(It.IsAny<object>())).Returns(expected);

            var result = await _sut.UpdateAsync(dto);

            Assert.Equal(expected, result);
        }
    }
}
