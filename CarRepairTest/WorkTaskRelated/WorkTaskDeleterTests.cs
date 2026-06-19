using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Moq;
using Service.WorkTaskRelated;
using System.Linq.Expressions;

namespace CarRepairTest.WorkTaskRelated
{
    public class WorkTaskDeleterTests
    {
        private readonly Mock<IWorkTaskRepository> _workTaskRepoMock = new();
        private readonly Mock<IEmployeeBookingRepository> _employeeBookingRepoMock = new();
        private readonly WorkTaskDeleter _sut;

        public WorkTaskDeleterTests()
        {
            _sut = new WorkTaskDeleter(_workTaskRepoMock.Object, _employeeBookingRepoMock.Object);
        }

        private void SetupTask(WorkTask? task)
        {
            _workTaskRepoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<WorkTask, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<WorkTask, object>>[]>()))
                .ReturnsAsync(task);
        }

        private void SetupBookings(List<EmployeeBooking> bookings)
        {
            _employeeBookingRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<EmployeeBooking, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<EmployeeBooking, object>>[]>()))
                .ReturnsAsync(bookings);
        }

        // --- DeleteAsync ---

        [Fact]
        public async Task DeleteAsync_ShouldRemoveAllEmployeeBookings_WhenTaskHasBookings()
        {
            var bookings = new List<EmployeeBooking> { new() { Id = 1 }, new() { Id = 2 } };
            SetupBookings(bookings);
            SetupTask(new WorkTask { Id = 1 });

            await _sut.DeleteAsync(1);

            _employeeBookingRepoMock.Verify(r => r.Remove(It.IsAny<EmployeeBooking>()), Times.Exactly(2));
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveTask_WhenTaskExists()
        {
            var task = new WorkTask { Id = 1 };
            SetupBookings(new List<EmployeeBooking>());
            SetupTask(task);

            await _sut.DeleteAsync(1);

            _workTaskRepoMock.Verify(r => r.Remove(task), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotCallRemoveOnTask_WhenTaskNotFound()
        {
            SetupBookings(new List<EmployeeBooking>());
            SetupTask(null);

            await _sut.DeleteAsync(99);

            _workTaskRepoMock.Verify(r => r.Remove(It.IsAny<WorkTask>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallSaveAsync_Always()
        {
            SetupBookings(new List<EmployeeBooking>());
            SetupTask(null);

            await _sut.DeleteAsync(1);

            _workTaskRepoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        // --- CancelAsync ---

        [Fact]
        public async Task CancelAsync_ShouldNotThrow_WhenTaskNotFound()
        {
            SetupTask(null);

            var exception = await Record.ExceptionAsync(() => _sut.CancelAsync(99));

            Assert.Null(exception);
        }

        [Fact]
        public async Task CancelAsync_ShouldNotCallSaveAsync_WhenTaskNotFound()
        {
            SetupTask(null);

            await _sut.CancelAsync(99);

            _workTaskRepoMock.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task CancelAsync_ShouldSetStatusToCancelled_WhenTaskExists()
        {
            var task = new WorkTask { Id = 1, Status = Model.DomainModel.TaskStatus.Assigned };
            SetupTask(task);

            await _sut.CancelAsync(1);

            Assert.Equal(Model.DomainModel.TaskStatus.Cancelled, task.Status);
        }

        [Fact]
        public async Task CancelAsync_ShouldCallSaveAsync_WhenTaskExists()
        {
            SetupTask(new WorkTask { Id = 1 });

            await _sut.CancelAsync(1);

            _workTaskRepoMock.Verify(r => r.SaveAsync(), Times.Once);
        }
    }
}
