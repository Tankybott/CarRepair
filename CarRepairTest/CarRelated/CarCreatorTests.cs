using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.CarRelated;
using System.Linq.Expressions;

namespace CarRepairTest.CarRelated
{
    public class CarCreatorTests
    {
        private readonly Mock<ICarRepository> _carRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly CarCreator _sut;

        public CarCreatorTests()
        {
            _sut = new CarCreator(_carRepositoryMock.Object, _mapperMock.Object);
        }

        private void SetupDefaultMocks(IntranetCarUpsertDto dto, Car car, IntranetCarReadDto readDto)
        {
            _mapperMock.Setup(m => m.Map<Car>(dto)).Returns(car);
            _carRepositoryMock
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Car, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Car, object>>[]>()))
                .ReturnsAsync(car);
            _mapperMock.Setup(m => m.Map<IntranetCarReadDto>(It.IsAny<object>())).Returns(readDto);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddEntityToRepository_WhenDtoIsValid()
        {
            var dto = new IntranetCarUpsertDto { Brand = "Toyota" };
            var car = new Car { Id = 1 };
            SetupDefaultMocks(dto, car, new IntranetCarReadDto());

            await _sut.CreateAsync(dto);

            _carRepositoryMock.Verify(r => r.Add(car), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCallSaveAsync_WhenDtoIsValid()
        {
            var dto = new IntranetCarUpsertDto { Brand = "Toyota" };
            var car = new Car { Id = 1 };
            SetupDefaultMocks(dto, car, new IntranetCarReadDto());

            await _sut.CreateAsync(dto);

            _carRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnMappedReadDto_WhenCarIsCreated()
        {
            var dto = new IntranetCarUpsertDto { Brand = "Toyota" };
            var car = new Car { Id = 1 };
            var expected = new IntranetCarReadDto { Id = 1, Brand = "Toyota" };
            SetupDefaultMocks(dto, car, expected);

            var result = await _sut.CreateAsync(dto);

            Assert.Equal(expected, result);
        }
    }
}
