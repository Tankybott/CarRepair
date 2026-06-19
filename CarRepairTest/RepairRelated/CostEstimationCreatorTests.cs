using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.PartRelated.Interface;
using Service.RepairRelated;
using System.Linq.Expressions;

namespace CarRepairTest.RepairRelated
{
    public class CostEstimationCreatorTests
    {
        private readonly Mock<IRepairRepository> _repairRepositoryMock = new();
        private readonly Mock<ICostEstimationItemRepository> _costItemRepositoryMock = new();
        private readonly Mock<IPartCreator> _partCreatorMock = new();
        private readonly CostEstimationCreator _sut;

        public CostEstimationCreatorTests()
        {
            _sut = new CostEstimationCreator(
                _repairRepositoryMock.Object,
                _costItemRepositoryMock.Object,
                _partCreatorMock.Object);
        }

        private void SetupRepair(Repair? repair)
        {
            _repairRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Repair, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Repair, object>>[]>()))
                .ReturnsAsync(repair);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddServiceCostItemForEachServiceCost_WhenServiceCostsProvided()
        {
            var dto = new IntranetEstimateCostsDto
            {
                RepairId = 1,
                ServiceCosts = new List<IntranetServiceCostItemDto>
                {
                    new() { ServiceName = "Oil Change", Cost = 50 },
                    new() { ServiceName = "Brake Check", Cost = 30 }
                }
            };
            var addedItems = new List<CostEstimationItem>();
            _costItemRepositoryMock.Setup(r => r.Add(It.IsAny<CostEstimationItem>())).Callback<CostEstimationItem>(addedItems.Add);
            SetupRepair(new Repair { Id = 1 });

            await _sut.CreateAsync(dto);

            Assert.Equal(2, addedItems.Count(i => i.Type == CostEstimationItemType.Service));
        }

        [Fact]
        public async Task CreateAsync_ShouldAddPartCostItemForEachPart_WhenPartsProvided()
        {
            var dto = new IntranetEstimateCostsDto
            {
                RepairId = 1,
                Parts = new List<IntranetPartEstimateItemDto>
                {
                    new() { Name = "Filter", UnitPrice = 20, Quantity = 1 },
                    new() { Name = "Pad", UnitPrice = 40, Quantity = 2 }
                }
            };
            _partCreatorMock.Setup(p => p.CreateAsync(It.IsAny<IntranetPartUpsertDto>())).ReturnsAsync(new IntranetPartReadDto());
            var addedItems = new List<CostEstimationItem>();
            _costItemRepositoryMock.Setup(r => r.Add(It.IsAny<CostEstimationItem>())).Callback<CostEstimationItem>(addedItems.Add);
            SetupRepair(new Repair { Id = 1 });

            await _sut.CreateAsync(dto);

            Assert.Equal(2, addedItems.Count(i => i.Type == CostEstimationItemType.Part));
        }

        [Fact]
        public async Task CreateAsync_ShouldCallPartCreatorForEachPart_WhenPartsProvided()
        {
            var dto = new IntranetEstimateCostsDto
            {
                RepairId = 1,
                Parts = new List<IntranetPartEstimateItemDto>
                {
                    new() { Name = "Filter", SerialNumber = "A1" },
                    new() { Name = "Pad", SerialNumber = "B2" }
                }
            };
            _partCreatorMock.Setup(p => p.CreateAsync(It.IsAny<IntranetPartUpsertDto>())).ReturnsAsync(new IntranetPartReadDto());
            SetupRepair(new Repair { Id = 1 });

            await _sut.CreateAsync(dto);

            _partCreatorMock.Verify(p => p.CreateAsync(It.IsAny<IntranetPartUpsertDto>()), Times.Exactly(2));
        }

        [Fact]
        public async Task CreateAsync_ShouldNotCallPartCreator_WhenPartsAreEmpty()
        {
            var dto = new IntranetEstimateCostsDto { RepairId = 1 };
            SetupRepair(new Repair { Id = 1 });

            await _sut.CreateAsync(dto);

            _partCreatorMock.Verify(p => p.CreateAsync(It.IsAny<IntranetPartUpsertDto>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddOtherCostItemForEachOtherCost_WhenOtherCostsProvided()
        {
            var dto = new IntranetEstimateCostsDto
            {
                RepairId = 1,
                OtherCosts = new List<IntranetOtherCostItemDto>
                {
                    new() { Name = "Disposal fee", Cost = 15 },
                    new() { Name = "Shipping", Cost = 10 }
                }
            };
            var addedItems = new List<CostEstimationItem>();
            _costItemRepositoryMock.Setup(r => r.Add(It.IsAny<CostEstimationItem>())).Callback<CostEstimationItem>(addedItems.Add);
            SetupRepair(new Repair { Id = 1 });

            await _sut.CreateAsync(dto);

            Assert.Equal(2, addedItems.Count(i => i.Type == CostEstimationItemType.Other));
        }

        [Fact]
        public async Task CreateAsync_ShouldSetPredictedPriceCorrectly_WhenAllCostTypesProvided()
        {
            var repair = new Repair { Id = 1 };
            var dto = new IntranetEstimateCostsDto
            {
                RepairId = 1,
                ServiceCosts = new List<IntranetServiceCostItemDto> { new() { Cost = 100 } },
                Parts = new List<IntranetPartEstimateItemDto> { new() { UnitPrice = 30, Quantity = 2 } },
                OtherCosts = new List<IntranetOtherCostItemDto> { new() { Cost = 20 } }
            };
            _partCreatorMock.Setup(p => p.CreateAsync(It.IsAny<IntranetPartUpsertDto>())).ReturnsAsync(new IntranetPartReadDto());
            SetupRepair(repair);

            await _sut.CreateAsync(dto);

            Assert.Equal(180m, repair.PredictedPrice);
        }

        [Fact]
        public async Task CreateAsync_ShouldSetStatusToManagerReviewed_WhenRepairExists()
        {
            var repair = new Repair { Id = 1, Status = RepairStatus.Pending };
            var dto = new IntranetEstimateCostsDto { RepairId = 1 };
            SetupRepair(repair);

            await _sut.CreateAsync(dto);

            Assert.Equal(RepairStatus.ManagerReviewed, repair.Status);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotUpdateRepairStatus_WhenRepairNotFound()
        {
            var dto = new IntranetEstimateCostsDto { RepairId = 99 };
            SetupRepair(null);

            var exception = await Record.ExceptionAsync(() => _sut.CreateAsync(dto));

            Assert.Null(exception);
        }

        [Fact]
        public async Task CreateAsync_ShouldCallSaveAsync_Always()
        {
            var dto = new IntranetEstimateCostsDto { RepairId = 1 };
            SetupRepair(new Repair { Id = 1 });

            await _sut.CreateAsync(dto);

            _costItemRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCalculatePartCostAsUnitPriceTimesQuantity_WhenPartProvided()
        {
            var repair = new Repair { Id = 1 };
            var dto = new IntranetEstimateCostsDto
            {
                RepairId = 1,
                Parts = new List<IntranetPartEstimateItemDto> { new() { Name = "Pad", UnitPrice = 25, Quantity = 4 } }
            };
            var addedItems = new List<CostEstimationItem>();
            _costItemRepositoryMock.Setup(r => r.Add(It.IsAny<CostEstimationItem>())).Callback<CostEstimationItem>(addedItems.Add);
            _partCreatorMock.Setup(p => p.CreateAsync(It.IsAny<IntranetPartUpsertDto>())).ReturnsAsync(new IntranetPartReadDto());
            SetupRepair(repair);

            await _sut.CreateAsync(dto);

            Assert.Equal(100m, addedItems.Single(i => i.Type == CostEstimationItemType.Part).Cost);
        }
    }
}
