using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.RepairRelated;

namespace CarRepairTest.RepairRelated
{
    public class RepairReaderTests
    {
        private readonly Mock<IRepairRepository> _repairRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly RepairReadService _sut;

        public RepairReaderTests()
        {
            _sut = new RepairReadService(_repairRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllForIndex_ShouldReturnMappedDtos_WhenRepairsExist()
        {
            var repairs = new List<Repair> { new Repair { Id = 1 } };
            var expected = new List<IntranetRepairReadDto> { new IntranetRepairReadDto { Id = 1 } };
            _repairRepositoryMock.Setup(r => r.GetAllWithDetailsAsync()).ReturnsAsync(repairs);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<Repair>, IEnumerable<IntranetRepairReadDto>>(repairs))
                .Returns(expected);

            var result = await _sut.GetAllForIndex();

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetAllForIndex_ShouldReturnEmptyCollection_WhenNoRepairsExist()
        {
            var repairs = new List<Repair>();
            _repairRepositoryMock.Setup(r => r.GetAllWithDetailsAsync()).ReturnsAsync(repairs);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<Repair>, IEnumerable<IntranetRepairReadDto>>(repairs))
                .Returns(new List<IntranetRepairReadDto>());

            var result = await _sut.GetAllForIndex();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetForCarAndUser_ShouldReturnMappedDtos_WhenRepairsExist()
        {
            var repairs = new List<Repair> { new Repair { Id = 1 } };
            var expected = new List<IntranetRepairReadDto> { new IntranetRepairReadDto { Id = 1 } };
            _repairRepositoryMock.Setup(r => r.GetAllForCarAndUserAsync(1, "user-123")).ReturnsAsync(repairs);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<Repair>, IEnumerable<IntranetRepairReadDto>>(repairs))
                .Returns(expected);

            var result = await _sut.GetForCarAndUser(1, "user-123");

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetManageForUser_ShouldReturnNull_WhenRepairNotFound()
        {
            _repairRepositoryMock.Setup(r => r.GetForManageAsync(It.IsAny<int>())).ReturnsAsync((Repair?)null);

            var result = await _sut.GetManageForUser(99, "user-123");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetManageForUser_ShouldReturnNull_WhenRepairBelongsToDifferentUser()
        {
            var repair = new Repair
            {
                Id = 1,
                Car = new Car { Client = new ClientProfile { ApplicationUserId = "other-user" } }
            };
            _repairRepositoryMock.Setup(r => r.GetForManageAsync(1)).ReturnsAsync(repair);

            var result = await _sut.GetManageForUser(1, "user-123");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetManageForUser_ShouldReturnMappedDto_WhenRepairBelongsToUser()
        {
            var repair = new Repair
            {
                Id = 1,
                Car = new Car { Client = new ClientProfile { ApplicationUserId = "user-123" } }
            };
            var expected = new IntranetRepairManageDto { RepairId = 1 };
            _repairRepositoryMock.Setup(r => r.GetForManageAsync(1)).ReturnsAsync(repair);
            _mapperMock.Setup(m => m.Map<IntranetRepairManageDto>(repair)).Returns(expected);

            var result = await _sut.GetManageForUser(1, "user-123");

            Assert.Equal(expected, result);
        }
    }
}
