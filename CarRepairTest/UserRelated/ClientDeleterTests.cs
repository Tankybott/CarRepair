using DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Model.DomainModel;
using Moq;
using Service.CarRelated.Interface;
using Service.UserRelated;
using System.Linq.Expressions;

namespace CarRepairTest.UserRelated
{
    public class ClientDeleterTests
    {
        private readonly Mock<IClientProfileRepository> _clientProfileRepoMock = new();
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<ICarRepository> _carRepoMock = new();
        private readonly Mock<ICarDeleter> _carDeleterMock = new();
        private readonly ClientDeleter _sut;

        public ClientDeleterTests()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            _sut = new ClientDeleter(
                _clientProfileRepoMock.Object,
                _userManagerMock.Object,
                _carRepoMock.Object,
                _carDeleterMock.Object);
        }

        private void SetupProfile(ClientProfile? profile)
        {
            _clientProfileRepoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<ClientProfile, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ClientProfile, object>>[]>()))
                .ReturnsAsync(profile);
        }

        private void SetupCars(List<Car> cars)
        {
            _carRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Car, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Car, object>>[]>()))
                .ReturnsAsync(cars);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenProfileNotFound()
        {
            SetupProfile(null);

            await Assert.ThrowsAsync<Exception>(() => _sut.DeleteAsync(1));
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallCarDeleterDeleteAsync_PerCar_WhenCarsExist()
        {
            SetupProfile(new ClientProfile { Id = 1, ApplicationUserId = "user-1" });
            SetupCars(new List<Car> { new() { Id = 10 }, new() { Id = 20 } });
            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);

            await _sut.DeleteAsync(1);

            _carDeleterMock.Verify(d => d.DeleteAsync(It.IsAny<int>()), Times.Exactly(2));
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotCallCarDeleter_WhenNoCarsExist()
        {
            SetupProfile(new ClientProfile { Id = 1, ApplicationUserId = "user-1" });
            SetupCars(new List<Car>());
            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);

            await _sut.DeleteAsync(1);

            _carDeleterMock.Verify(d => d.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSetDeletedAt_WhenProfileFound()
        {
            var profile = new ClientProfile { Id = 1, ApplicationUserId = "user-1" };
            SetupProfile(profile);
            SetupCars(new List<Car>());
            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);

            await _sut.DeleteAsync(1);

            Assert.NotNull(profile.DeletedAt);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallSaveAsync_WhenProfileFound()
        {
            SetupProfile(new ClientProfile { Id = 1, ApplicationUserId = "user-1" });
            SetupCars(new List<Car>());
            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);

            await _sut.DeleteAsync(1);

            _clientProfileRepoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallUserManagerDeleteAsync_WhenUserFound()
        {
            var user = new ApplicationUser { Id = "user-1" };
            SetupProfile(new ClientProfile { Id = 1, ApplicationUserId = "user-1" });
            SetupCars(new List<Car>());
            _userManagerMock.Setup(u => u.FindByIdAsync("user-1")).ReturnsAsync(user);
            _userManagerMock.Setup(u => u.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            await _sut.DeleteAsync(1);

            _userManagerMock.Verify(u => u.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotCallUserManagerDeleteAsync_WhenUserNotFound()
        {
            SetupProfile(new ClientProfile { Id = 1, ApplicationUserId = "user-1" });
            SetupCars(new List<Car>());
            _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);

            await _sut.DeleteAsync(1);

            _userManagerMock.Verify(u => u.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }
    }
}
