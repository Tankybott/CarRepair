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
    public class ClientCreatorTests
    {
        private readonly Mock<IClientProfileRepository> _clientProfileRepoMock = new();
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly ClientCreator _sut;

        public ClientCreatorTests()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            _sut = new ClientCreator(
                _clientProfileRepoMock.Object,
                _userManagerMock.Object,
                _mapperMock.Object,
                Mock.Of<ILogger<ClientCreator>>());
        }

        private static IntranetClientCreateDto BuildDto() => new()
        {
            Email = "test@example.com",
            Password = "Password1!",
            Name = "John",
            Surname = "Doe",
            PhoneNumber = "123456789"
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
        public async Task CreateAsync_ShouldCallAddToRoleAsync_WithCustomerRole_WhenUserCreatedSuccessfully()
        {
            SetupCreateSucceeds();
            _mapperMock.Setup(m => m.Map<IntranetClientReadDto>(It.IsAny<object>())).Returns(new IntranetClientReadDto());

            await _sut.CreateAsync(BuildDto());

            _userManagerMock.Verify(u => u.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Customer"), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCallAdd_OnClientProfileRepository_WhenUserCreatedSuccessfully()
        {
            SetupCreateSucceeds();
            _mapperMock.Setup(m => m.Map<IntranetClientReadDto>(It.IsAny<object>())).Returns(new IntranetClientReadDto());

            await _sut.CreateAsync(BuildDto());

            _clientProfileRepoMock.Verify(r => r.Add(It.IsAny<ClientProfile>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCallSaveAsync_WhenUserCreatedSuccessfully()
        {
            SetupCreateSucceeds();
            _mapperMock.Setup(m => m.Map<IntranetClientReadDto>(It.IsAny<object>())).Returns(new IntranetClientReadDto());

            await _sut.CreateAsync(BuildDto());

            _clientProfileRepoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnMappedDto_WhenCreationSucceeds()
        {
            SetupCreateSucceeds();
            var expected = new IntranetClientReadDto { Id = 1, Name = "John" };
            _mapperMock.Setup(m => m.Map<IntranetClientReadDto>(It.IsAny<object>())).Returns(expected);

            var result = await _sut.CreateAsync(BuildDto());

            Assert.Equal(expected, result);
        }
    }
}
