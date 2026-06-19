using DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Model.DomainModel;
using Moq;
using Service.UserRelated;
using System.Linq.Expressions;

namespace CarRepairTest.UserRelated
{
    public class EmployeeDeleterTests
    {
        private readonly Mock<IEmployeeProfileRepository> _employeeRepoMock = new();
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly EmployeeDeleter _sut;

        public EmployeeDeleterTests()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            _sut = new EmployeeDeleter(_employeeRepoMock.Object, _userManagerMock.Object);
        }

        private void SetupProfile(EmployeeProfile? profile)
        {
            _employeeRepoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<EmployeeProfile, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<EmployeeProfile, object>>[]>()))
                .ReturnsAsync(profile);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenProfileNotFound()
        {
            SetupProfile(null);

            await Assert.ThrowsAsync<Exception>(() => _sut.DeleteAsync(1));
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallSetLockoutEnabled_WhenUserFound()
        {
            var user = new ApplicationUser { Id = "user-1" };
            SetupProfile(new EmployeeProfile { Id = 1, ApplicationUserId = "user-1" });
            _userManagerMock.Setup(u => u.FindByIdAsync("user-1")).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.SetLockoutEnabledAsync(user, true)).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(u => u.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue)).ReturnsAsync(IdentityResult.Success);

            await _sut.DeleteAsync(1);

            _userManagerMock.Verify(u => u.SetLockoutEnabledAsync(user, true), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallSetLockoutEndDateWithMaxValue_WhenUserFound()
        {
            var user = new ApplicationUser { Id = "user-1" };
            SetupProfile(new EmployeeProfile { Id = 1, ApplicationUserId = "user-1" });
            _userManagerMock.Setup(u => u.FindByIdAsync("user-1")).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.SetLockoutEnabledAsync(user, true)).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(u => u.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue)).ReturnsAsync(IdentityResult.Success);

            await _sut.DeleteAsync(1);

            _userManagerMock.Verify(u => u.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotCallLockout_WhenUserNotFound()
        {
            SetupProfile(new EmployeeProfile { Id = 1, ApplicationUserId = "user-1" });
            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);

            await _sut.DeleteAsync(1);

            _userManagerMock.Verify(u => u.SetLockoutEnabledAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ShouldClearSpecializations_WhenProfileFound()
        {
            var profile = new EmployeeProfile { Id = 1, ApplicationUserId = "user-1" };
            profile.Specializations.Add(new ServiceType { Id = 5 });
            SetupProfile(profile);
            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);

            await _sut.DeleteAsync(1);

            Assert.Empty(profile.Specializations);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRemove_WhenProfileFound()
        {
            var profile = new EmployeeProfile { Id = 1, ApplicationUserId = "user-1" };
            SetupProfile(profile);
            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);

            await _sut.DeleteAsync(1);

            _employeeRepoMock.Verify(r => r.Remove(profile), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallSaveAsync_WhenProfileFound()
        {
            SetupProfile(new EmployeeProfile { Id = 1, ApplicationUserId = "user-1" });
            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);

            await _sut.DeleteAsync(1);

            _employeeRepoMock.Verify(r => r.SaveAsync(), Times.Once);
        }
    }
}
