using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.WebsiteConfigRelated.Interface;
using Service.WorkTaskRelated;
using System.Linq.Expressions;

namespace CarRepairTest.WorkTaskRelated
{
    public class WorkTaskUpdaterTests
    {
        private readonly Mock<IWorkTaskRepository> _workTaskRepoMock = new();
        private readonly Mock<IEmployeeProfileRepository> _employeeProfileRepoMock = new();
        private readonly Mock<IEmployeeBookingRepository> _employeeBookingRepoMock = new();
        private readonly Mock<IWorkingHoursValidator> _validatorMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly WorkTaskUpdater _sut;

        public WorkTaskUpdaterTests()
        {
            _sut = new WorkTaskUpdater(
                _workTaskRepoMock.Object,
                _employeeProfileRepoMock.Object,
                _employeeBookingRepoMock.Object,
                _validatorMock.Object,
                _mapperMock.Object);
        }

        private static IntranetWorkTaskUpdateDto BuildDto(List<int>? employeeIds = null) => new()
        {
            Id = 1,
            Description = "Updated desc",
            PredictedStart = new DateTime(2025, 6, 9, 9, 0, 0),
            PredictedEnd = new DateTime(2025, 6, 9, 16, 0, 0),
            AssignedEmployeeIds = employeeIds ?? new List<int>()
        };

        private void SetupTaskAndBookings(WorkTask? task, List<EmployeeBooking>? existingBookings = null)
        {
            _workTaskRepoMock.Setup(r => r.GetTrackedWithEmployeesAsync(It.IsAny<int>())).ReturnsAsync(task);
            _employeeBookingRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<EmployeeBooking, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<EmployeeBooking, object>>[]>()))
                .ReturnsAsync(existingBookings ?? new List<EmployeeBooking>());
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallWorkingHoursValidator()
        {
            var dto = BuildDto();
            SetupTaskAndBookings(new WorkTask { Id = 1 });
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(new IntranetWorkTaskReadDto());

            await _sut.UpdateAsync(dto);

            _validatorMock.Verify(v => v.ValidateAsync(dto.PredictedStart, dto.PredictedEnd), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowInvalidOperationException_WhenTaskNotFound()
        {
            var dto = BuildDto();
            SetupTaskAndBookings(null);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.UpdateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateTaskProperties_WhenTaskExists()
        {
            var task = new WorkTask { Id = 1, Description = "Old" };
            var dto = BuildDto();
            SetupTaskAndBookings(task);
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(new IntranetWorkTaskReadDto());

            await _sut.UpdateAsync(dto);

            Assert.Equal("Updated desc", task.Description);
            Assert.Equal(dto.PredictedStart, task.PredictedStart);
            Assert.Equal(dto.PredictedEnd, task.PredicetedEnd);
        }

        [Fact]
        public async Task UpdateAsync_ShouldClearExistingEmployees_WhenUpdating()
        {
            var task = new WorkTask
            {
                Id = 1,
                EmployeesAssigned = new List<EmployeeProfile> { new() { Id = 5 } }
            };
            var dto = BuildDto();
            SetupTaskAndBookings(task);
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(new IntranetWorkTaskReadDto());

            await _sut.UpdateAsync(dto);

            Assert.Empty(task.EmployeesAssigned);
        }

        [Fact]
        public async Task UpdateAsync_ShouldAddNewEmployees_WhenEmployeeIdsProvided()
        {
            var task = new WorkTask { Id = 1 };
            var dto = BuildDto(new List<int> { 10, 20 });
            var employees = new List<EmployeeProfile> { new() { Id = 10 }, new() { Id = 20 } };
            SetupTaskAndBookings(task);
            _employeeProfileRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<EmployeeProfile, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<EmployeeProfile, object>>[]>()))
                .ReturnsAsync(employees);
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(new IntranetWorkTaskReadDto());

            await _sut.UpdateAsync(dto);

            Assert.Equal(2, task.EmployeesAssigned.Count);
        }

        [Fact]
        public async Task UpdateAsync_ShouldRemoveOldBookings_WhenTaskHasExistingBookings()
        {
            var task = new WorkTask { Id = 1 };
            var oldBookings = new List<EmployeeBooking> { new() { Id = 50 }, new() { Id = 51 } };
            var dto = BuildDto();
            SetupTaskAndBookings(task, oldBookings);
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(new IntranetWorkTaskReadDto());

            await _sut.UpdateAsync(dto);

            _employeeBookingRepoMock.Verify(r => r.Remove(It.IsAny<EmployeeBooking>()), Times.Exactly(2));
        }

        [Fact]
        public async Task UpdateAsync_ShouldAddNewBookingPerEmployee_WhenEmployeesProvided()
        {
            var task = new WorkTask { Id = 1 };
            var dto = BuildDto(new List<int> { 10, 20 });
            SetupTaskAndBookings(task);
            _employeeProfileRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<EmployeeProfile, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<EmployeeProfile, object>>[]>()))
                .ReturnsAsync(new List<EmployeeProfile> { new() { Id = 10 }, new() { Id = 20 } });
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(new IntranetWorkTaskReadDto());

            await _sut.UpdateAsync(dto);

            _employeeBookingRepoMock.Verify(r => r.Add(It.IsAny<EmployeeBooking>()), Times.Exactly(2));
        }

        [Fact]
        public async Task UpdateAsync_ShouldNotAddNewBookings_WhenNoEmployeesProvided()
        {
            var task = new WorkTask { Id = 1 };
            var dto = BuildDto();
            SetupTaskAndBookings(task);
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(new IntranetWorkTaskReadDto());

            await _sut.UpdateAsync(dto);

            _employeeBookingRepoMock.Verify(r => r.Add(It.IsAny<EmployeeBooking>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallSaveAsync_WhenTaskExists()
        {
            var task = new WorkTask { Id = 1 };
            var dto = BuildDto();
            SetupTaskAndBookings(task);
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(new IntranetWorkTaskReadDto());

            await _sut.UpdateAsync(dto);

            _workTaskRepoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnMappedDto_WhenTaskIsUpdated()
        {
            var task = new WorkTask { Id = 1 };
            var dto = BuildDto();
            var expected = new IntranetWorkTaskReadDto { Id = 1 };
            SetupTaskAndBookings(task);
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(task)).Returns(expected);

            var result = await _sut.UpdateAsync(dto);

            Assert.Equal(expected, result);
        }
    }
}
