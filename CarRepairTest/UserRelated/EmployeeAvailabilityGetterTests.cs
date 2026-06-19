using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Moq;
using Service.UserRelated;

namespace CarRepairTest.UserRelated
{
    public class EmployeeAvailabilityGetterTests
    {
        private readonly Mock<IEmployeeProfileRepository> _employeeProfileRepoMock = new();
        private readonly EmployeeAvailabilityGetter _sut;

        private static readonly DateTime Start = new(2025, 6, 9, 9, 0, 0);
        private static readonly DateTime End = new(2025, 6, 9, 16, 0, 0);

        public EmployeeAvailabilityGetterTests()
        {
            _sut = new EmployeeAvailabilityGetter(_employeeProfileRepoMock.Object);
        }

        private void SetupEmployees(List<EmployeeProfile> employees)
        {
            _employeeProfileRepoMock
                .Setup(r => r.GetAvailableForServiceAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int?>()))
                .ReturnsAsync(employees);
        }

        [Fact]
        public async Task GetAvailableAsync_ShouldReturnDtoWithCorrectId_WhenEmployeeExists()
        {
            SetupEmployees(new List<EmployeeProfile> { new() { Id = 42, Name = "Alice", Surname = "Smith" } });

            var result = (await _sut.GetAvailableAsync(1, Start, End)).ToList();

            Assert.Equal(42, result[0].Id);
        }

        [Fact]
        public async Task GetAvailableAsync_ShouldReturnDtoWithCorrectEmployeeNumber_WhenEmployeeExists()
        {
            SetupEmployees(new List<EmployeeProfile> { new() { Id = 1, EmployeeNumber = "EMP099" } });

            var result = (await _sut.GetAvailableAsync(1, Start, End)).ToList();

            Assert.Equal("EMP099", result[0].EmployeeNumber);
        }

        [Fact]
        public async Task GetAvailableAsync_ShouldBuildFullNameFromNameAndSurname_WhenBothPresent()
        {
            SetupEmployees(new List<EmployeeProfile> { new() { Id = 1, Name = "Alice", Surname = "Smith" } });

            var result = (await _sut.GetAvailableAsync(1, Start, End)).ToList();

            Assert.Equal("Alice Smith", result[0].FullName);
        }

        [Fact]
        public async Task GetAvailableAsync_ShouldUseEmptyStringForName_WhenNameIsNull()
        {
            SetupEmployees(new List<EmployeeProfile> { new() { Id = 1, Name = null, Surname = "Smith" } });

            var result = (await _sut.GetAvailableAsync(1, Start, End)).ToList();

            Assert.Equal("Smith", result[0].FullName);
        }

        [Fact]
        public async Task GetAvailableAsync_ShouldUseEmptyStringForSurname_WhenSurnameIsNull()
        {
            SetupEmployees(new List<EmployeeProfile> { new() { Id = 1, Name = "Alice", Surname = null } });

            var result = (await _sut.GetAvailableAsync(1, Start, End)).ToList();

            Assert.Equal("Alice", result[0].FullName);
        }

        [Fact]
        public async Task GetAvailableAsync_ShouldReturnEmptyFullName_WhenBothNameAndSurnameAreNull()
        {
            SetupEmployees(new List<EmployeeProfile> { new() { Id = 1, Name = null, Surname = null } });

            var result = (await _sut.GetAvailableAsync(1, Start, End)).ToList();

            Assert.Equal(string.Empty, result[0].FullName);
        }

        [Fact]
        public async Task GetAvailableAsync_ShouldReturnEmptyList_WhenNoEmployeesAvailable()
        {
            SetupEmployees(new List<EmployeeProfile>());

            var result = await _sut.GetAvailableAsync(1, Start, End);

            Assert.Empty(result);
        }
    }
}
