using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.RepairRelated;

namespace CarRepairTest.RepairRelated
{
    public class OverheadCostAdderTests
    {
        private readonly Mock<ICostEstimationItemRepository> _repoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly OverheadCostAdder _sut;

        public OverheadCostAdderTests()
        {
            _sut = new OverheadCostAdder(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldAddItemWithOverheadType()
        {
            var dto = new IntranetOverheadCostUpsertDto { RepairId = 1, Name = "Labour", Cost = 200 };
            CostEstimationItem? captured = null;
            _repoMock.Setup(r => r.Add(It.IsAny<CostEstimationItem>())).Callback<CostEstimationItem>(i => captured = i);
            _mapperMock.Setup(m => m.Map<IntranetCostEstimationItemReadDto>(It.IsAny<object>())).Returns(new IntranetCostEstimationItemReadDto());

            await _sut.AddAsync(dto);

            Assert.Equal(CostEstimationItemType.Overhead, captured!.Type);
        }

        [Fact]
        public async Task AddAsync_ShouldAddItemWithCorrectProperties_WhenDtoIsValid()
        {
            var dto = new IntranetOverheadCostUpsertDto { RepairId = 5, Name = "Labour", Description = "Extra", Cost = 200 };
            CostEstimationItem? captured = null;
            _repoMock.Setup(r => r.Add(It.IsAny<CostEstimationItem>())).Callback<CostEstimationItem>(i => captured = i);
            _mapperMock.Setup(m => m.Map<IntranetCostEstimationItemReadDto>(It.IsAny<object>())).Returns(new IntranetCostEstimationItemReadDto());

            await _sut.AddAsync(dto);

            Assert.Equal(5, captured!.RepairId);
            Assert.Equal("Labour", captured.Name);
            Assert.Equal("Extra", captured.Description);
            Assert.Equal(200m, captured.Cost);
        }

        [Fact]
        public async Task AddAsync_ShouldCallSaveAsync_WhenDtoIsValid()
        {
            var dto = new IntranetOverheadCostUpsertDto { Name = "Labour" };
            _mapperMock.Setup(m => m.Map<IntranetCostEstimationItemReadDto>(It.IsAny<object>())).Returns(new IntranetCostEstimationItemReadDto());

            await _sut.AddAsync(dto);

            _repoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnMappedDto_WhenItemIsAdded()
        {
            var dto = new IntranetOverheadCostUpsertDto { Name = "Labour", Cost = 200 };
            var expected = new IntranetCostEstimationItemReadDto { Name = "Labour" };
            _mapperMock.Setup(m => m.Map<IntranetCostEstimationItemReadDto>(It.IsAny<object>())).Returns(expected);

            var result = await _sut.AddAsync(dto);

            Assert.Equal(expected, result);
        }
    }
}
