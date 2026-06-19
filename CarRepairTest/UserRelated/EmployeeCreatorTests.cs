using AutoMapper;
using DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.UserRelated;
using System.Linq.Expressions;

namespace CarRepairTest.UserRelated
{
    public class EmployeeCreatorTests
    {
        private readonly Mock<IEmployeeProfileRepository> _employeeRepoMock = new();
        private readonly Mock<IServiceTypeRepository> _serviceTypeRepoMock = new();
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly EmployeeCreator _sut;

        public EmployeeCreatorTests()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            _sut = new EmployeeCreator(
                _employeeRepoMock.Object,
                _serviceTypeRepoMock.Object,
                _userManagerMock.Object,
                _mapperMock.Object,
                Mock.Of<ILogger<EmployeeCreator>>());
        }

        private static IntranetEmployeeCreateDto BuildDto(string role = "Employee", List<int>? specializationIds = null) => new()
        {
            Email = "emp@example.com",
            Password = "Password1!",
            Name = "Alice",
            Surname = "Tech",
            EmployeeNumber = "EMP001",
            Role = role,
            SpecializationIds = specializationIds ?? new List<int>()
        };

        private void SetupCreateSucceeds()
        {
            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowInvalidOperationException_WhenUserManagerCreateFails()
        {
            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "error" }));

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.CreateAsync(BuildDto()));
        }

        [Fact]
        public async Task CreateAsync_ShouldCallAddToRoleAsync_WithDtoRole_WhenUserCreatedSuccessfully()
        {
            SetupCreateSucceeds();
            _mapperMock.Setup(m => m.Map<IntranetEmployeeReadDto>(It.IsAny<object>())).Returns(new IntranetEmployeeReadDto());

            await _sut.CreateAsync(BuildDto(role: "Manager"));

            _userManagerMock.Verify(u => u.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Manager"), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldFetchSpecializations_WhenSpecializationIdsProvided()
        {
            SetupCreateSucceeds();
            _serviceTypeRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()))
                .ReturnsAsync(new List<ServiceType> { new() { Id = 1 } });
            _mapperMock.Setup(m => m.Map<IntranetEmployeeReadDto>(It.IsAny<object>())).Returns(new IntranetEmployeeReadDto());

            await _sut.CreateAsync(BuildDto(specializationIds: new List<int> { 1 }));

            _serviceTypeRepoMock.Verify(
                r => r.GetAllAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotFetchSpecializations_WhenSpecializationIdsEmpty()
        {
            SetupCreateSucceeds();
            _mapperMock.Setup(m => m.Map<IntranetEmployeeReadDto>(It.IsAny<object>())).Returns(new IntranetEmployeeReadDto());

            await _sut.CreateAsync(BuildDto());

            _serviceTypeRepoMock.Verify(
                r => r.GetAllAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldCallAdd_WhenProfileCreated()
        {
            SetupCreateSucceeds();
            _mapperMock.Setup(m => m.Map<IntranetEmployeeReadDto>(It.IsAny<object>())).Returns(new IntranetEmployeeReadDto());

            await _sut.CreateAsync(BuildDto());

            _employeeRepoMock.Verify(r => r.Add(It.IsAny<EmployeeProfile>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCallSaveAsync_WhenProfileCreated()
        {
            SetupCreateSucceeds();
            _mapperMock.Setup(m => m.Map<IntranetEmployeeReadDto>(It.IsAny<object>())).Returns(new IntranetEmployeeReadDto());

            await _sut.CreateAsync(BuildDto());

            _employeeRepoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnMappedDto_WhenCreationSucceeds()
        {
            SetupCreateSucceeds();
            var expected = new IntranetEmployeeReadDto { Id = 1, Name = "Alice" };
            _mapperMock.Setup(m => m.Map<IntranetEmployeeReadDto>(It.IsAny<object>())).Returns(expected);

            var result = await _sut.CreateAsync(BuildDto());

            Assert.Equal(expected, result);
        }
    }
}
