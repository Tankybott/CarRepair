using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Model.DTOs.PortalDto;
using Moq;
using Service.CarRelated;
using System.Linq.Expressions;

namespace CarRepairTest.CarRelated
{
    public class CarReaderTests
    {
        private readonly Mock<ICarRepository> _carRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly CarReadService _sut;

        public CarReaderTests()
        {
            _sut = new CarReadService(_carRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllForIndex_ShouldReturnMappedDtos_WhenCarsExist()
        {
            var cars = new List<Car> { new Car { Id = 1, Brand = "Toyota" } };
            var expected = new List<IntranetCarReadDto> { new IntranetCarReadDto { Id = 1, Brand = "Toyota" } };
            _carRepositoryMock
                .Setup(r => r.GetAllAsync(
                    It.IsAny<Expression<Func<Car, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Car, object>>[]>()))
                .ReturnsAsync(cars);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<Car>, IEnumerable<IntranetCarReadDto>>(cars))
                .Returns(expected);

            var result = await _sut.GetAllForIndex();

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetAllForIndex_ShouldReturnEmptyCollection_WhenNoCarsExist()
        {
            var cars = new List<Car>();
            _carRepositoryMock
                .Setup(r => r.GetAllAsync(
                    It.IsAny<Expression<Func<Car, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Car, object>>[]>()))
                .ReturnsAsync(cars);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<Car>, IEnumerable<IntranetCarReadDto>>(cars))
                .Returns(new List<IntranetCarReadDto>());

            var result = await _sut.GetAllForIndex();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCarsForUser_ShouldReturnMappedDtos_WhenUserHasCars()
        {
            var userId = "user-123";
            var cars = new List<Car> { new Car { Id = 1 } };
            var expected = new List<PortalCarDto> { new PortalCarDto { Id = 1 } };
            _carRepositoryMock
                .Setup(r => r.GetAllAsync(
                    It.IsAny<Expression<Func<Car, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Car, object>>[]>()))
                .ReturnsAsync(cars);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<Car>, IEnumerable<PortalCarDto>>(cars))
                .Returns(expected);

            var result = await _sut.GetCarsForUser(userId);

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetCarForUser_ShouldReturnNull_WhenCarNotFound()
        {
            _carRepositoryMock
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Car, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Car, object>>[]>()))
                .ReturnsAsync((Car?)null);

            var result = await _sut.GetCarForUser(99, "user-123");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetCarForUser_ShouldReturnMappedDto_WhenCarExists()
        {
            var car = new Car { Id = 1 };
            var expected = new CarAndRepairCarDto { Id = 1 };
            _carRepositoryMock
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Car, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Car, object>>[]>()))
                .ReturnsAsync(car);
            _mapperMock.Setup(m => m.Map<CarAndRepairCarDto>(It.IsAny<object>())).Returns(expected);

            var result = await _sut.GetCarForUser(1, "user-123");

            Assert.Equal(expected, result);
        }
    }
}
