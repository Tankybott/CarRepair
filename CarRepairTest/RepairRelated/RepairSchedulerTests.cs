using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.RepairRelated;
using Service.RepairRelated.Interface;
using System.Linq.Expressions;

namespace CarRepairTest.RepairRelated
{
    public class RepairSchedulerTests
    {
        private readonly Mock<IRepairBookingRepository> _repairBookingRepositoryMock = new();
        private readonly Mock<IRepairStatusUpdater> _repairStatusUpdaterMock = new();
        private readonly Mock<IWorkTaskRepository> _workTaskRepositoryMock = new();
        private readonly RepairScheduler _sut;

        public RepairSchedulerTests()
        {
            _sut = new RepairScheduler(
                _repairBookingRepositoryMock.Object,
                _repairStatusUpdaterMock.Object,
                _workTaskRepositoryMock.Object);
        }

        private void SetupTasks(IEnumerable<WorkTask> tasks)
        {
            _workTaskRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<WorkTask, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<WorkTask, object>>[]>()))
                .ReturnsAsync(tasks);
        }

        private void SetupExistingBooking(RepairBooking? booking)
        {
            _repairBookingRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<RepairBooking, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<RepairBooking, object>>[]>()))
                .ReturnsAsync(booking);
        }

        private static IntranetRepairScheduleDto BuildDto(int repairId, DateTime delivery, DateTime pickup) =>
            new IntranetRepairScheduleDto
            {
                RepairId = repairId,
                CarDeliveryDateTime = delivery,
                CarPickupDateTime = pickup
            };

        private static WorkTask BuildTask(DateTime start, DateTime end) =>
            new WorkTask { PredictedStart = start, PredicetedEnd = end };

        [Fact]
        public async Task ScheduleAsync_ShouldThrow_WhenDeliveryEqualsEarliestTaskStart()
        {
            var taskStart = new DateTime(2025, 6, 10, 8, 0, 0);
            SetupTasks(new List<WorkTask> { BuildTask(taskStart, taskStart.AddHours(2)) });
            var dto = BuildDto(1, delivery: taskStart, pickup: taskStart.AddHours(4));

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.ScheduleAsync(dto));
        }

        [Fact]
        public async Task ScheduleAsync_ShouldThrow_WhenDeliveryIsAfterEarliestTaskStart()
        {
            var taskStart = new DateTime(2025, 6, 10, 8, 0, 0);
            SetupTasks(new List<WorkTask> { BuildTask(taskStart, taskStart.AddHours(2)) });
            var dto = BuildDto(1, delivery: taskStart.AddMinutes(1), pickup: taskStart.AddHours(4));

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.ScheduleAsync(dto));
        }

        [Fact]
        public async Task ScheduleAsync_ShouldThrow_WhenPickupEqualsLatestTaskEnd()
        {
            var taskStart = new DateTime(2025, 6, 10, 8, 0, 0);
            var taskEnd = taskStart.AddHours(2);
            SetupTasks(new List<WorkTask> { BuildTask(taskStart, taskEnd) });
            var dto = BuildDto(1, delivery: taskStart.AddHours(-1), pickup: taskEnd);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.ScheduleAsync(dto));
        }

        [Fact]
        public async Task ScheduleAsync_ShouldThrow_WhenPickupIsBeforeLatestTaskEnd()
        {
            var taskStart = new DateTime(2025, 6, 10, 8, 0, 0);
            var taskEnd = taskStart.AddHours(2);
            SetupTasks(new List<WorkTask> { BuildTask(taskStart, taskEnd) });
            var dto = BuildDto(1, delivery: taskStart.AddHours(-1), pickup: taskEnd.AddMinutes(-1));

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.ScheduleAsync(dto));
        }

        [Fact]
        public async Task ScheduleAsync_ShouldNotValidateTimes_WhenNoActiveTasksExist()
        {
            SetupTasks(new List<WorkTask>());
            SetupExistingBooking(null);
            var dto = BuildDto(1, delivery: DateTime.UtcNow, pickup: DateTime.UtcNow.AddDays(-1));

            var exception = await Record.ExceptionAsync(() => _sut.ScheduleAsync(dto));

            Assert.Null(exception);
        }

        [Fact]
        public async Task ScheduleAsync_ShouldAddNewBooking_WhenNoExistingBookingExists()
        {
            SetupTasks(new List<WorkTask>());
            SetupExistingBooking(null);
            var delivery = new DateTime(2025, 6, 10);
            var pickup = new DateTime(2025, 6, 12);
            var dto = BuildDto(1, delivery, pickup);

            await _sut.ScheduleAsync(dto);

            _repairBookingRepositoryMock.Verify(r => r.Add(It.Is<RepairBooking>(b =>
                b.RepairId == 1 &&
                b.StartDateTime == delivery &&
                b.EndDateTime == pickup)), Times.Once);
        }

        [Fact]
        public async Task ScheduleAsync_ShouldUpdateExistingBooking_WhenBookingAlreadyExists()
        {
            var existingBooking = new RepairBooking { Id = 5, RepairId = 1 };
            SetupTasks(new List<WorkTask>());
            SetupExistingBooking(existingBooking);
            var delivery = new DateTime(2025, 6, 10);
            var pickup = new DateTime(2025, 6, 12);
            var dto = BuildDto(1, delivery, pickup);

            await _sut.ScheduleAsync(dto);

            Assert.Equal(delivery, existingBooking.StartDateTime);
            Assert.Equal(pickup, existingBooking.EndDateTime);
        }

        [Fact]
        public async Task ScheduleAsync_ShouldNotAddNewBooking_WhenExistingBookingIsUpdated()
        {
            SetupTasks(new List<WorkTask>());
            SetupExistingBooking(new RepairBooking { Id = 5 });
            var dto = BuildDto(1, DateTime.UtcNow, DateTime.UtcNow.AddDays(2));

            await _sut.ScheduleAsync(dto);

            _repairBookingRepositoryMock.Verify(r => r.Add(It.IsAny<RepairBooking>()), Times.Never);
        }

        [Fact]
        public async Task ScheduleAsync_ShouldCallSaveAsync_WhenSchedulingSucceeds()
        {
            SetupTasks(new List<WorkTask>());
            SetupExistingBooking(null);
            var dto = BuildDto(1, DateTime.UtcNow, DateTime.UtcNow.AddDays(2));

            await _sut.ScheduleAsync(dto);

            _repairBookingRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task ScheduleAsync_ShouldUpdateRepairStatusToScheduled_WhenSchedulingSucceeds()
        {
            SetupTasks(new List<WorkTask>());
            SetupExistingBooking(null);
            var dto = BuildDto(1, DateTime.UtcNow, DateTime.UtcNow.AddDays(2));

            await _sut.ScheduleAsync(dto);

            _repairStatusUpdaterMock.Verify(s => s.UpdateStatusAsync(1, RepairStatus.Scheduled), Times.Once);
        }

        [Fact]
        public async Task ScheduleAsync_ShouldUseMostConstrainingTaskBounds_WhenMultipleTasksExist()
        {
            var early = new DateTime(2025, 6, 10, 8, 0, 0);
            var late = new DateTime(2025, 6, 10, 14, 0, 0);
            var tasks = new List<WorkTask>
            {
                BuildTask(early, early.AddHours(2)),
                BuildTask(late, late.AddHours(2))
            };
            SetupTasks(tasks);
            var dto = BuildDto(1, delivery: early, pickup: late.AddHours(3));

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.ScheduleAsync(dto));
        }
    }
}
