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
    public class WorkTaskCreatorTests
    {
        private readonly Mock<IWorkTaskRepository> _workTaskRepoMock = new();
        private readonly Mock<IEmployeeProfileRepository> _employeeProfileRepoMock = new();
        private readonly Mock<IEmployeeBookingRepository> _employeeBookingRepoMock = new();
        private readonly Mock<IWorkingHoursValidator> _validatorMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly WorkTaskCreator _sut;

        public WorkTaskCreatorTests()
        {
            _sut = new WorkTaskCreator(
                _workTaskRepoMock.Object,
                _employeeProfileRepoMock.Object,
                _employeeBookingRepoMock.Object,
                _validatorMock.Object,
                _mapperMock.Object);
        }

        private static IntranetWorkTaskCreateDto BuildDto(List<int>? employeeIds = null) => new()
        {
            RepairId = 1,
            Description = "Fix engine",
            PredictedStart = new DateTime(2025, 6, 9, 9, 0, 0),
            PredictedEnd = new DateTime(2025, 6, 9, 16, 0, 0),
            AssignedEmployeeIds = employeeIds ?? new List<int>()
        };

        [Fact]
        public async Task CreateAsync_ShouldCallWorkingHoursValidator_WhenCreatingTask()
        {
            var dto = BuildDto();
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(new IntranetWorkTaskReadDto());

            await _sut.CreateAsync(dto);

            _validatorMock.Verify(v => v.ValidateAsync(dto.PredictedStart, dto.PredictedEnd), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddTaskWithAssignedStatus()
        {
            var dto = BuildDto();
            WorkTask? captured = null;
            _workTaskRepoMock.Setup(r => r.Add(It.IsAny<WorkTask>())).Callback<WorkTask>(t => captured = t);
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(new IntranetWorkTaskReadDto());

            await _sut.CreateAsync(dto);

            Assert.Equal(Model.DomainModel.TaskStatus.Assigned, captured!.Status);
        }

        [Fact]
        public async Task CreateAsync_ShouldSetTaskProperties_WhenDtoIsValid()
        {
            var dto = BuildDto();
            WorkTask? captured = null;
            _workTaskRepoMock.Setup(r => r.Add(It.IsAny<WorkTask>())).Callback<WorkTask>(t => captured = t);
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(new IntranetWorkTaskReadDto());

            await _sut.CreateAsync(dto);

            Assert.Equal(dto.RepairId, captured!.RepairId);
            Assert.Equal(dto.Description, captured.Description);
            Assert.Equal(dto.PredictedStart, captured.PredictedStart);
            Assert.Equal(dto.PredictedEnd, captured.PredicetedEnd);
        }

        [Fact]
        public async Task CreateAsync_ShouldFetchAndAttachEmployees_WhenEmployeeIdsProvided()
        {
            var dto = BuildDto(new List<int> { 1, 2 });
            var employees = new List<EmployeeProfile> { new() { Id = 1 }, new() { Id = 2 } };
            _employeeProfileRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<EmployeeProfile, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<EmployeeProfile, object>>[]>()))
                .ReturnsAsync(employees);
            WorkTask? captured = null;
            _workTaskRepoMock.Setup(r => r.Add(It.IsAny<WorkTask>())).Callback<WorkTask>(t => captured = t);
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(new IntranetWorkTaskReadDto());

            await _sut.CreateAsync(dto);

            Assert.Equal(2, captured!.EmployeesAssigned.Count);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotFetchEmployees_WhenNoEmployeeIdsProvided()
        {
            var dto = BuildDto();
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(new IntranetWorkTaskReadDto());

            await _sut.CreateAsync(dto);

            _employeeProfileRepoMock.Verify(
                r => r.GetAllAsync(It.IsAny<Expression<Func<EmployeeProfile, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<EmployeeProfile, object>>[]>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddTaskToRepository()
        {
            var dto = BuildDto();
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(new IntranetWorkTaskReadDto());

            await _sut.CreateAsync(dto);

            _workTaskRepoMock.Verify(r => r.Add(It.IsAny<WorkTask>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCallWorkTaskSaveAsync()
        {
            var dto = BuildDto();
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(new IntranetWorkTaskReadDto());

            await _sut.CreateAsync(dto);

            _workTaskRepoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddEmployeeBookingPerEmployee_WhenEmployeesAssigned()
        {
            var dto = BuildDto(new List<int> { 10, 20 });
            _employeeProfileRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<EmployeeProfile, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<EmployeeProfile, object>>[]>()))
                .ReturnsAsync(new List<EmployeeProfile> { new() { Id = 10 }, new() { Id = 20 } });
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(new IntranetWorkTaskReadDto());

            await _sut.CreateAsync(dto);

            _employeeBookingRepoMock.Verify(r => r.Add(It.IsAny<EmployeeBooking>()), Times.Exactly(2));
        }

        [Fact]
        public async Task CreateAsync_ShouldSaveEmployeeBookings_WhenEmployeesAssigned()
        {
            var dto = BuildDto(new List<int> { 10 });
            _employeeProfileRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<EmployeeProfile, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<EmployeeProfile, object>>[]>()))
                .ReturnsAsync(new List<EmployeeProfile> { new() { Id = 10 } });
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(new IntranetWorkTaskReadDto());

            await _sut.CreateAsync(dto);

            _employeeBookingRepoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotSaveEmployeeBookings_WhenNoEmployeesAssigned()
        {
            var dto = BuildDto();
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(new IntranetWorkTaskReadDto());

            await _sut.CreateAsync(dto);

            _employeeBookingRepoMock.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnMappedDto_WhenTaskIsCreated()
        {
            var dto = BuildDto();
            var expected = new IntranetWorkTaskReadDto { Id = 1 };
            _mapperMock.Setup(m => m.Map<IntranetWorkTaskReadDto>(It.IsAny<object>())).Returns(expected);

            var result = await _sut.CreateAsync(dto);

            Assert.Equal(expected, result);
        }
    }
}
