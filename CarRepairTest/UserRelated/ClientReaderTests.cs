using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.UserRelated;
using System.Linq.Expressions;

namespace CarRepairTest.UserRelated
{
    public class ClientReaderTests
    {
        private readonly Mock<IClientProfileRepository> _clientProfileRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly ClientReadService _sut;

        public ClientReaderTests()
        {
            _sut = new ClientReadService(_clientProfileRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllForIndex_ShouldReturnMappedDtos_WhenProfilesExist()
        {
            var profiles = new List<ClientProfile> { new() { Id = 1, Name = "John" } };
            var expected = new List<IntranetClientReadDto> { new() { Id = 1, Name = "John" } };
            _clientProfileRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<ClientProfile, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ClientProfile, object>>[]>()))
                .ReturnsAsync(profiles);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<ClientProfile>, IEnumerable<IntranetClientReadDto>>(profiles))
                .Returns(expected);

            var result = await _sut.GetAllForIndex();

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetAllForIndex_ShouldReturnEmptyCollection_WhenNoProfilesExist()
        {
            var profiles = new List<ClientProfile>();
            _clientProfileRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<ClientProfile, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ClientProfile, object>>[]>()))
                .ReturnsAsync(profiles);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<ClientProfile>, IEnumerable<IntranetClientReadDto>>(profiles))
                .Returns(new List<IntranetClientReadDto>());

            var result = await _sut.GetAllForIndex();

            Assert.Empty(result);
        }
    }
}
