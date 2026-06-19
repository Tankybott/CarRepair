using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.UserRelated;
using System.Linq.Expressions;

namespace CarRepairTest.UserRelated
{
    public class ClientUpdaterTests
    {
        private readonly Mock<IClientProfileRepository> _clientProfileRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly ClientUpdater _sut;

        public ClientUpdaterTests()
        {
            _sut = new ClientUpdater(_clientProfileRepoMock.Object, _mapperMock.Object);
        }

        private void SetupProfile(ClientProfile? profile)
        {
            _clientProfileRepoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<ClientProfile, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ClientProfile, object>>[]>()))
                .ReturnsAsync(profile);
        }

        private static IntranetClientUpdateDto BuildDto(int id = 1) => new()
        {
            Id = id,
            Name = "Jane",
            Surname = "Smith",
            PhoneNumber = "987654321"
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
            var profile = new ClientProfile { Id = 1 };
            SetupProfile(profile);
            _mapperMock.Setup(m => m.Map<IntranetClientReadDto>(It.IsAny<object>())).Returns(new IntranetClientReadDto());

            await _sut.UpdateAsync(BuildDto());

            Assert.Equal("Jane", profile.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateSurname_WhenProfileFound()
        {
            var profile = new ClientProfile { Id = 1 };
            SetupProfile(profile);
            _mapperMock.Setup(m => m.Map<IntranetClientReadDto>(It.IsAny<object>())).Returns(new IntranetClientReadDto());

            await _sut.UpdateAsync(BuildDto());

            Assert.Equal("Smith", profile.Surname);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdatePhoneNumber_WhenProfileFound()
        {
            var profile = new ClientProfile { Id = 1 };
            SetupProfile(profile);
            _mapperMock.Setup(m => m.Map<IntranetClientReadDto>(It.IsAny<object>())).Returns(new IntranetClientReadDto());

            await _sut.UpdateAsync(BuildDto());

            Assert.Equal("987654321", profile.PhoneNumber);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallUpdate_WhenProfileFound()
        {
            var profile = new ClientProfile { Id = 1 };
            SetupProfile(profile);
            _mapperMock.Setup(m => m.Map<IntranetClientReadDto>(It.IsAny<object>())).Returns(new IntranetClientReadDto());

            await _sut.UpdateAsync(BuildDto());

            _clientProfileRepoMock.Verify(r => r.Update(profile), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallSaveAsync_WhenProfileFound()
        {
            SetupProfile(new ClientProfile { Id = 1 });
            _mapperMock.Setup(m => m.Map<IntranetClientReadDto>(It.IsAny<object>())).Returns(new IntranetClientReadDto());

            await _sut.UpdateAsync(BuildDto());

            _clientProfileRepoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnMappedDto_WhenUpdateSucceeds()
        {
            SetupProfile(new ClientProfile { Id = 1 });
            var expected = new IntranetClientReadDto { Id = 1, Name = "Jane" };
            _mapperMock.Setup(m => m.Map<IntranetClientReadDto>(It.IsAny<object>())).Returns(expected);

            var result = await _sut.UpdateAsync(BuildDto());

            Assert.Equal(expected, result);
        }
    }
}
