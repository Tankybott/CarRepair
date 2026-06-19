using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Moq;
using Service.RepairRelated;
using System.Linq.Expressions;

namespace CarRepairTest.RepairRelated
{
    public class OverheadCostDeleterTests
    {
        private readonly Mock<ICostEstimationItemRepository> _repoMock = new();
        private readonly OverheadCostDeleter _sut;

        public OverheadCostDeleterTests()
        {
            _sut = new OverheadCostDeleter(_repoMock.Object);
        }

        [Fact]
        public async Task DeleteAsync_ShouldSetDeletedAt_WhenItemExists()
        {
            var item = new CostEstimationItem { Id = 1 };
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<CostEstimationItem, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<CostEstimationItem, object>>[]>()))
                .ReturnsAsync(item);

            await _sut.DeleteAsync(1);

            Assert.NotNull(item.DeletedAt);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallSaveAsync_WhenItemExists()
        {
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<CostEstimationItem, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<CostEstimationItem, object>>[]>()))
                .ReturnsAsync(new CostEstimationItem { Id = 1 });

            await _sut.DeleteAsync(1);

            _repoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotCallSaveAsync_WhenItemNotFound()
        {
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<CostEstimationItem, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<CostEstimationItem, object>>[]>()))
                .ReturnsAsync((CostEstimationItem?)null);

            await _sut.DeleteAsync(99);

            _repoMock.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ShouldNotThrow_WhenItemNotFound()
        {
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<CostEstimationItem, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<CostEstimationItem, object>>[]>()))
                .ReturnsAsync((CostEstimationItem?)null);

            var exception = await Record.ExceptionAsync(() => _sut.DeleteAsync(99));

            Assert.Null(exception);
        }
    }
}
