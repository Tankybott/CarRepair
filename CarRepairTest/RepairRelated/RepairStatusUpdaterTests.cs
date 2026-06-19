using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Moq;
using Service.RepairRelated;
using System.Linq.Expressions;

namespace CarRepairTest.RepairRelated
{
    public class RepairStatusUpdaterTests
    {
        private readonly Mock<IRepairRepository> _repairRepositoryMock = new();
        private readonly RepairStatusUpdater _sut;

        public RepairStatusUpdaterTests()
        {
            _sut = new RepairStatusUpdater(_repairRepositoryMock.Object);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldNotCallSaveAsync_WhenRepairNotFound()
        {
            _repairRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Repair, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Repair, object>>[]>()))
                .ReturnsAsync((Repair?)null);

            await _sut.UpdateStatusAsync(99, RepairStatus.Done);

            _repairRepositoryMock.Verify(r => r.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldNotThrow_WhenRepairNotFound()
        {
            _repairRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Repair, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Repair, object>>[]>()))
                .ReturnsAsync((Repair?)null);

            var exception = await Record.ExceptionAsync(() => _sut.UpdateStatusAsync(99, RepairStatus.Done));

            Assert.Null(exception);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldUpdateStatus_WhenRepairExists()
        {
            var repair = new Repair { Id = 1, Status = RepairStatus.Pending };
            _repairRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Repair, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Repair, object>>[]>()))
                .ReturnsAsync(repair);

            await _sut.UpdateStatusAsync(1, RepairStatus.Done);

            Assert.Equal(RepairStatus.Done, repair.Status);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldCallSaveAsync_WhenRepairExists()
        {
            var repair = new Repair { Id = 1 };
            _repairRepositoryMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Repair, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Repair, object>>[]>()))
                .ReturnsAsync(repair);

            await _sut.UpdateStatusAsync(1, RepairStatus.Done);

            _repairRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
        }
    }
}
