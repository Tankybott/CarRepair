using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Moq;
using Service.CarRelated;
using Service.RepairRelated.Interface;
using System.Linq.Expressions;

namespace CarRepairTest.CarRelated
{
    public class CarDeleterTests
    {
        private readonly Mock<ICarRepository> _carRepositoryMock = new();
        private readonly Mock<IRepairRepository> _repairRepositoryMock = new();
        private readonly Mock<IRepairDeleter> _repairDeleterMock = new();
        private readonly CarDeleter _sut;

        public CarDeleterTests()
        {
            _sut = new CarDeleter(_carRepositoryMock.Object, _repairRepositoryMock.Object, _repairDeleterMock.Object);
        }

        private void SetupRepairs(IEnumerable<Repair> repairs)
        {
            _repairRepositoryMock
                .Setup(r => r.GetAllAsync(
                    It.IsAny<Expression<Func<Repair, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Repair, object>>[]>()))
                .ReturnsAsync(repairs);
        }

        private void SetupCar(Car? car)
        {
            _carRepositoryMock
                .Setup(r => r.GetAsync(
                    It.IsAny<Expression<Func<Car, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Car, object>>[]>()))
                .ReturnsAsync(car);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepairDeleterForEachRepair_WhenCarHasActiveRepairs()
        {
            var repairs = new List<Repair> { new Repair { Id = 10 }, new Repair { Id = 20 } };
            SetupRepairs(repairs);
            SetupCar(new Car { Id = 1 });

            await _sut.DeleteAsync(1);

            _repairDeleterMock.Verify(d => d.DeleteAsync(It.IsAny<int>()), Times.Exactly(2));
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotCallRepairDeleter_WhenCarHasNoActiveRepairs()
        {
            SetupRepairs(new List<Repair>());
            SetupCar(new Car { Id = 1 });

            await _sut.DeleteAsync(1);

            _repairDeleterMock.Verify(d => d.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenCarNotFound()
        {
            SetupRepairs(new List<Repair>());
            SetupCar(null);

            await Assert.ThrowsAsync<Exception>(() => _sut.DeleteAsync(99));
        }

        [Fact]
        public async Task DeleteAsync_ShouldSetDeletedAt_WhenCarExists()
        {
            var car = new Car { Id = 1 };
            SetupRepairs(new List<Repair>());
            SetupCar(car);

            await _sut.DeleteAsync(1);

            Assert.NotNull(car.DeletedAt);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallSaveAsync_WhenCarExists()
        {
            SetupRepairs(new List<Repair>());
            SetupCar(new Car { Id = 1 });

            await _sut.DeleteAsync(1);

            _carRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
        }
    }
}
