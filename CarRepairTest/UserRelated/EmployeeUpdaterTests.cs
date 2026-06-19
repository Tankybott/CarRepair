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
    public class EmployeeUpdaterTests
    {
        private readonly Mock<IEmployeeProfileRepository> _employeeRepoMock = new();
        private readonly Mock<IServiceTypeRepository> _serviceTypeRepoMock = new();
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly EmployeeUpdater _sut;

        public EmployeeUpdaterTests()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            _sut = new EmployeeUpdater(
                _employeeRepoMock.Object,
                _serviceTypeRepoMock.Object,
                _userManagerMock.Object,
                _mapperMock.Object,
                Mock.Of<ILogger<EmployeeUpdater>>());
        }

        private void SetupProfile(EmployeeProfile? profile)
        {
            _employeeRepoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<EmployeeProfile, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<EmployeeProfile, object>>[]>()))
                .ReturnsAsync(profile);
        }

        private static IntranetEmployeeUpdateDto BuildDto(int id = 1, string? newPassword = null, List<int>? specializationIds = null) => new()
        {
            Id = id,
            Name = "Bob",
            Surname = "Builder",
            EmployeeNumber = "EMP002",
            NewPassword = newPassword,
            SpecializationIds = specializationIds ?? new List<int>()
        };

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenProfileNotFound()
        {
            SetupProfile(null);

            await Assert.ThrowsAsync<Exception>(() => _sut.UpdateAsync(BuildDto()));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateName_WhenProfileFound()
        {
            var profile = new EmployeeProfile { Id = 1 };
            SetupProfile(profile);
            _mapperMock.Setup(m => m.Map<IntranetEmployeeReadDto>(It.IsAny<object>())).Returns(new IntranetEmployeeReadDto());

            await _sut.UpdateAsync(BuildDto());

            Assert.Equal("Bob", profile.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateSurname_WhenProfileFound()
        {
            var profile = new EmployeeProfile { Id = 1 };
            SetupProfile(profile);
            _mapperMock.Setup(m => m.Map<IntranetEmployeeReadDto>(It.IsAny<object>())).Returns(new IntranetEmployeeReadDto());

            await _sut.UpdateAsync(BuildDto());

            Assert.Equal("Builder", profile.Surname);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateEmployeeNumber_WhenProfileFound()
        {
            var profile = new EmployeeProfile { Id = 1 };
            SetupProfile(profile);
            _mapperMock.Setup(m => m.Map<IntranetEmployeeReadDto>(It.IsAny<object>())).Returns(new IntranetEmployeeReadDto());

            await _sut.UpdateAsync(BuildDto());

            Assert.Equal("EMP002", profile.EmployeeNumber);
        }

        [Fact]
        public async Task UpdateAsync_ShouldClearSpecializations_WhenUpdating()
        {
            var profile = new EmployeeProfile { Id = 1 };
            profile.Specializations.Add(new ServiceType { Id = 99 });
            SetupProfile(profile);
            _mapperMock.Setup(m => m.Map<IntranetEmployeeReadDto>(It.IsAny<object>())).Returns(new IntranetEmployeeReadDto());

            await _sut.UpdateAsync(BuildDto());

            Assert.Empty(profile.Specializations);
        }

        [Fact]
        public async Task UpdateAsync_ShouldFetchAndAssignNewSpecializations_WhenSpecializationIdsProvided()
        {
            var profile = new EmployeeProfile { Id = 1 };
            SetupProfile(profile);
            var specs = new List<ServiceType> { new() { Id = 1 }, new() { Id = 2 } };
            _serviceTypeRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()))
                .ReturnsAsync(specs);
            _mapperMock.Setup(m => m.Map<IntranetEmployeeReadDto>(It.IsAny<object>())).Returns(new IntranetEmployeeReadDto());

            await _sut.UpdateAsync(BuildDto(specializationIds: new List<int> { 1, 2 }));

            Assert.Equal(2, profile.Specializations.Count);
        }

        [Fact]
        public async Task UpdateAsync_ShouldNotFetchSpecializations_WhenSpecializationIdsEmpty()
        {
            SetupProfile(new EmployeeProfile { Id = 1 });
            _mapperMock.Setup(m => m.Map<IntranetEmployeeReadDto>(It.IsAny<object>())).Returns(new IntranetEmployeeReadDto());

            await _sut.UpdateAsync(BuildDto());

            _serviceTypeRepoMock.Verify(
                r => r.GetAllAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallUpdate_WhenProfileFound()
        {
            var profile = new EmployeeProfile { Id = 1 };
            SetupProfile(profile);
            _mapperMock.Setup(m => m.Map<IntranetEmployeeReadDto>(It.IsAny<object>())).Returns(new IntranetEmployeeReadDto());

            await _sut.UpdateAsync(BuildDto());

            _employeeRepoMock.Verify(r => r.Update(profile), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallSaveAsync_WhenProfileFound()
        {
            SetupProfile(new EmployeeProfile { Id = 1 });
            _mapperMock.Setup(m => m.Map<IntranetEmployeeReadDto>(It.IsAny<object>())).Returns(new IntranetEmployeeReadDto());

            await _sut.UpdateAsync(BuildDto());

            _employeeRepoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldNotResetPassword_WhenNewPasswordIsNull()
        {
            SetupProfile(new EmployeeProfile { Id = 1 });
            _mapperMock.Setup(m => m.Map<IntranetEmployeeReadDto>(It.IsAny<object>())).Returns(new IntranetEmployeeReadDto());

            await _sut.UpdateAsync(BuildDto(newPassword: null));

            _userManagerMock.Verify(u => u.GeneratePasswordResetTokenAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldResetPassword_WhenNewPasswordProvided()
        {
            var user = new ApplicationUser { Id = "user-1" };
            var profile = new EmployeeProfile { Id = 1, ApplicationUserId = "user-1", ApplicationUser = user };
            SetupProfile(profile);
            _userManagerMock.Setup(u => u.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("token");
            _userManagerMock.Setup(u => u.ResetPasswordAsync(user, "token", "NewPass1!")).ReturnsAsync(IdentityResult.Success);
            _mapperMock.Setup(m => m.Map<IntranetEmployeeReadDto>(It.IsAny<object>())).Returns(new IntranetEmployeeReadDto());

            await _sut.UpdateAsync(BuildDto(newPassword: "NewPass1!"));

            _userManagerMock.Verify(u => u.ResetPasswordAsync(user, It.IsAny<string>(), "NewPass1!"), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowInvalidOperationException_WhenPasswordResetFails()
        {
            var user = new ApplicationUser { Id = "user-1" };
            var profile = new EmployeeProfile { Id = 1, ApplicationUserId = "user-1", ApplicationUser = user };
            SetupProfile(profile);
            _userManagerMock.Setup(u => u.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("token");
            _userManagerMock.Setup(u => u.ResetPasswordAsync(user, It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Bad password" }));
            _mapperMock.Setup(m => m.Map<IntranetEmployeeReadDto>(It.IsAny<object>())).Returns(new IntranetEmployeeReadDto());

            await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.UpdateAsync(BuildDto(newPassword: "bad")));
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnMappedDto_WhenUpdateSucceeds()
        {
            SetupProfile(new EmployeeProfile { Id = 1 });
            var expected = new IntranetEmployeeReadDto { Id = 1, Name = "Bob" };
            _mapperMock.Setup(m => m.Map<IntranetEmployeeReadDto>(It.IsAny<object>())).Returns(expected);

            var result = await _sut.UpdateAsync(BuildDto());

            Assert.Equal(expected, result);
        }
    }
}
