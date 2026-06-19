using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Moq;
using Service.RepairRelated;
using System.Linq.Expressions;

namespace CarRepairTest.RepairRelated
{
    public class RepairDeleterTests
    {
        private readonly Mock<IRepairRepository> _repairRepositoryMock = new();
        private readonly Mock<IPartRepository> _partRepositoryMock = new();
        private readonly Mock<ICostEstimationItemRepository> _costItemRepositoryMock = new();
        private readonly Mock<IWorkTaskRepository> _workTaskRepositoryMock = new();
        private readonly Mock<IEmployeeBookingRepository> _employeeBookingRepositoryMock = new();
        private readonly Mock<IRepairBookingRepository> _repairBookingRepositoryMock = new();
        private readonly RepairDeleter _sut;

        public RepairDeleterTests()
        {
            _sut = new RepairDeleter(
                _repairRepositoryMock.Object,
                _partRepositoryMock.Object,
                _costItemRepositoryMock.Object,
                _workTaskRepositoryMock.Object,
                _employeeBookingRepositoryMock.Object,
                _repairBookingRepositoryMock.Object);
        }

        private void SetupEmptyDefaults()
        {
            _partRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Part, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Part, object>>[]>()))
                .ReturnsAsync(new List<Part>());
            _costItemRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<CostEstimationItem, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<CostEstimationItem, object>>[]>()))
                .ReturnsAsync(new List<CostEstimationItem>());
            _workTaskRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<WorkTask, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<WorkTask, object>>[]>()))
                .ReturnsAsync(new List<WorkTask>());
            _employeeBookingRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<EmployeeBooking, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<EmployeeBooking, object>>[]>()))
                .ReturnsAsync(new List<EmployeeBooking>());
            _repairBookingRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<RepairBooking, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<RepairBooking, object>>[]>()))
                .ReturnsAsync((RepairBooking?)null);
            _repairRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Repair, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Repair, object>>[]>()))
                .ReturnsAsync((Repair?)null);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteAllActiveParts_WhenRepairHasParts()
        {
            var parts = new List<Part> { new Part { Id = 10 }, new Part { Id = 11 } };
            SetupEmptyDefaults();
            _partRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Part, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Part, object>>[]>()))
                .ReturnsAsync(parts);

            await _sut.DeleteAsync(1);

            Assert.All(parts, p => Assert.NotNull(p.DeletedAt));
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteAllActiveCostItems_WhenRepairHasCostItems()
        {
            var costItems = new List<CostEstimationItem> { new CostEstimationItem { Id = 20 }, new CostEstimationItem { Id = 21 } };
            SetupEmptyDefaults();
            _costItemRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<CostEstimationItem, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<CostEstimationItem, object>>[]>()))
                .ReturnsAsync(costItems);

            await _sut.DeleteAsync(1);

            Assert.All(costItems, c => Assert.NotNull(c.DeletedAt));
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteAllActiveTasks_WhenRepairHasTasks()
        {
            var tasks = new List<WorkTask> { new WorkTask { Id = 30 }, new WorkTask { Id = 31 } };
            SetupEmptyDefaults();
            _workTaskRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<WorkTask, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<WorkTask, object>>[]>()))
                .ReturnsAsync(tasks);

            await _sut.DeleteAsync(1);

            Assert.All(tasks, t => Assert.NotNull(t.DeletedAt));
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteEmployeeBookingsForEachTask_WhenTasksHaveBookings()
        {
            var bookings = new List<EmployeeBooking> { new EmployeeBooking { Id = 40 }, new EmployeeBooking { Id = 41 } };
            var tasks = new List<WorkTask> { new WorkTask { Id = 30 } };
            SetupEmptyDefaults();
            _workTaskRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<WorkTask, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<WorkTask, object>>[]>()))
                .ReturnsAsync(tasks);
            _employeeBookingRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<EmployeeBooking, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<EmployeeBooking, object>>[]>()))
                .ReturnsAsync(bookings);

            await _sut.DeleteAsync(1);

            Assert.All(bookings, b => Assert.NotNull(b.DeletedAt));
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteRepairBooking_WhenRepairHasBooking()
        {
            var repairBooking = new RepairBooking { Id = 50 };
            SetupEmptyDefaults();
            _repairBookingRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<RepairBooking, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<RepairBooking, object>>[]>()))
                .ReturnsAsync(repairBooking);

            await _sut.DeleteAsync(1);

            Assert.NotNull(repairBooking.DeletedAt);
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotThrow_WhenRepairBookingNotFound()
        {
            SetupEmptyDefaults();

            var exception = await Record.ExceptionAsync(() => _sut.DeleteAsync(1));

            Assert.Null(exception);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSoftDeleteRepair_WhenRepairExists()
        {
            var repair = new Repair { Id = 1 };
            SetupEmptyDefaults();
            _repairRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Repair, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Repair, object>>[]>()))
                .ReturnsAsync(repair);

            await _sut.DeleteAsync(1);

            Assert.NotNull(repair.DeletedAt);
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotThrow_WhenRepairAlreadyDeleted()
        {
            SetupEmptyDefaults();

            var exception = await Record.ExceptionAsync(() => _sut.DeleteAsync(99));

            Assert.Null(exception);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallSaveAsync_Always()
        {
            SetupEmptyDefaults();

            await _sut.DeleteAsync(1);

            _repairRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
        }
    }
}
