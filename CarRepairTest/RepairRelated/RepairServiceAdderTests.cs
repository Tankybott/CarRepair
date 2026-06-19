using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Moq;
using Service.RepairRelated;
using System.Linq.Expressions;

namespace CarRepairTest.RepairRelated
{
    public class RepairServiceAdderTests
    {
        private readonly Mock<IRepairRepository> _repairRepositoryMock = new();
        private readonly Mock<IServiceRepository> _serviceRepositoryMock = new();
        private readonly RepairServiceAdder _sut;

        public RepairServiceAdderTests()
        {
            _sut = new RepairServiceAdder(_repairRepositoryMock.Object, _serviceRepositoryMock.Object);
        }

        [Fact]
        public async Task AddServicesAsync_ShouldDoNothing_WhenRepairNotFound()
        {
            _repairRepositoryMock.Setup(r => r.GetTrackedWithServicesAsync(It.IsAny<int>())).ReturnsAsync((Repair?)null);

            var exception = await Record.ExceptionAsync(() => _sut.AddServicesAsync(99, new List<int> { 1 }));

            Assert.Null(exception);
        }

        [Fact]
        public async Task AddServicesAsync_ShouldNotCallSaveAsync_WhenRepairNotFound()
        {
            _repairRepositoryMock.Setup(r => r.GetTrackedWithServicesAsync(It.IsAny<int>())).ReturnsAsync((Repair?)null);

            await _sut.AddServicesAsync(99, new List<int> { 1 });

            _repairRepositoryMock.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task AddServicesAsync_ShouldAddNewServices_WhenServicesNotAlreadyInRepair()
        {
            var repair = new Repair { Id = 1, ServicesSelected = new List<Model.DomainModel.Service>() };
            var services = new List<Model.DomainModel.Service> { new() { Id = 10 }, new() { Id = 11 } };
            _repairRepositoryMock.Setup(r => r.GetTrackedWithServicesAsync(1)).ReturnsAsync(repair);
            _serviceRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Model.DomainModel.Service, object>>[]>()))
                .ReturnsAsync(services);

            await _sut.AddServicesAsync(1, new List<int> { 10, 11 });

            Assert.Equal(2, repair.ServicesSelected.Count);
        }

        [Fact]
        public async Task AddServicesAsync_ShouldNotAddDuplicateServices_WhenServiceAlreadyInRepair()
        {
            var existingService = new Model.DomainModel.Service { Id = 10 };
            var repair = new Repair
            {
                Id = 1,
                ServicesSelected = new List<Model.DomainModel.Service> { existingService }
            };
            var services = new List<Model.DomainModel.Service> { existingService, new() { Id = 11 } };
            _repairRepositoryMock.Setup(r => r.GetTrackedWithServicesAsync(1)).ReturnsAsync(repair);
            _serviceRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Model.DomainModel.Service, object>>[]>()))
                .ReturnsAsync(services);

            await _sut.AddServicesAsync(1, new List<int> { 10, 11 });

            Assert.Equal(2, repair.ServicesSelected.Count);
        }

        [Fact]
        public async Task AddServicesAsync_ShouldCallSaveAsync_WhenRepairExists()
        {
            var repair = new Repair { Id = 1, ServicesSelected = new List<Model.DomainModel.Service>() };
            _repairRepositoryMock.Setup(r => r.GetTrackedWithServicesAsync(1)).ReturnsAsync(repair);
            _serviceRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Model.DomainModel.Service, object>>[]>()))
                .ReturnsAsync(new List<Model.DomainModel.Service>());

            await _sut.AddServicesAsync(1, new List<int>());

            _repairRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
        }
    }
}
