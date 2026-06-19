using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.RepairRelated;
using System.Linq.Expressions;

namespace CarRepairTest.RepairRelated
{
    public class RepairCreatorTests
    {
        private readonly Mock<IRepairRepository> _repairRepositoryMock = new();
        private readonly Mock<IServiceRepository> _serviceRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly RepairCreator _sut;

        public RepairCreatorTests()
        {
            _sut = new RepairCreator(_repairRepositoryMock.Object, _serviceRepositoryMock.Object, _mapperMock.Object);
        }

        private void SetupGetWithDetails(Repair repair)
        {
            _repairRepositoryMock
                .Setup(r => r.GetWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(repair);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddRepairWithPendingStatus_WhenDtoIsValid()
        {
            var dto = new IntranetRepairCreateDto { CarId = 1, ServiceIds = new List<int>() };
            Repair? captured = null;
            _repairRepositoryMock.Setup(r => r.Add(It.IsAny<Repair>())).Callback<Repair>(r => captured = r);
            SetupGetWithDetails(new Repair());
            _mapperMock.Setup(m => m.Map<IntranetRepairReadDto>(It.IsAny<object>())).Returns(new IntranetRepairReadDto());

            await _sut.CreateAsync(dto);

            Assert.Equal(RepairStatus.Pending, captured!.Status);
        }

        [Fact]
        public async Task CreateAsync_ShouldSetCarId_WhenDtoIsValid()
        {
            var dto = new IntranetRepairCreateDto { CarId = 42, ServiceIds = new List<int>() };
            Repair? captured = null;
            _repairRepositoryMock.Setup(r => r.Add(It.IsAny<Repair>())).Callback<Repair>(r => captured = r);
            SetupGetWithDetails(new Repair());
            _mapperMock.Setup(m => m.Map<IntranetRepairReadDto>(It.IsAny<object>())).Returns(new IntranetRepairReadDto());

            await _sut.CreateAsync(dto);

            Assert.Equal(42, captured!.CarId);
        }

        [Fact]
        public async Task CreateAsync_ShouldSetClientDescription_WhenDtoIsValid()
        {
            var dto = new IntranetRepairCreateDto { CarId = 1, ClientDescription = "Engine noise", ServiceIds = new List<int>() };
            Repair? captured = null;
            _repairRepositoryMock.Setup(r => r.Add(It.IsAny<Repair>())).Callback<Repair>(r => captured = r);
            SetupGetWithDetails(new Repair());
            _mapperMock.Setup(m => m.Map<IntranetRepairReadDto>(It.IsAny<object>())).Returns(new IntranetRepairReadDto());

            await _sut.CreateAsync(dto);

            Assert.Equal("Engine noise", captured!.ClientDescription);
        }

        [Fact]
        public async Task CreateAsync_ShouldFetchAndAttachServices_WhenServiceIdsAreProvided()
        {
            var dto = new IntranetRepairCreateDto { CarId = 1, ServiceIds = new List<int> { 1, 2 } };
            var services = new List<Model.DomainModel.Service> { new() { Id = 1 }, new() { Id = 2 } };
            Repair? captured = null;
            _repairRepositoryMock.Setup(r => r.Add(It.IsAny<Repair>())).Callback<Repair>(r => captured = r);
            _serviceRepositoryMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Model.DomainModel.Service, object>>[]>()))
                .ReturnsAsync(services);
            SetupGetWithDetails(new Repair());
            _mapperMock.Setup(m => m.Map<IntranetRepairReadDto>(It.IsAny<object>())).Returns(new IntranetRepairReadDto());

            await _sut.CreateAsync(dto);

            Assert.Equal(2, captured!.ServicesSelected.Count);
        }

        [Fact]
        public async Task CreateAsync_ShouldNotFetchServices_WhenServiceIdsAreEmpty()
        {
            var dto = new IntranetRepairCreateDto { CarId = 1, ServiceIds = new List<int>() };
            SetupGetWithDetails(new Repair());
            _mapperMock.Setup(m => m.Map<IntranetRepairReadDto>(It.IsAny<object>())).Returns(new IntranetRepairReadDto());

            await _sut.CreateAsync(dto);

            _serviceRepositoryMock.Verify(
                r => r.GetAllAsync(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Model.DomainModel.Service, object>>[]>()),
                Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ShouldCallSaveAsync_WhenDtoIsValid()
        {
            var dto = new IntranetRepairCreateDto { CarId = 1, ServiceIds = new List<int>() };
            SetupGetWithDetails(new Repair());
            _mapperMock.Setup(m => m.Map<IntranetRepairReadDto>(It.IsAny<object>())).Returns(new IntranetRepairReadDto());

            await _sut.CreateAsync(dto);

            _repairRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnMappedReadDto_WhenRepairIsCreated()
        {
            var dto = new IntranetRepairCreateDto { CarId = 1, ServiceIds = new List<int>() };
            var expected = new IntranetRepairReadDto { Id = 1 };
            SetupGetWithDetails(new Repair());
            _mapperMock.Setup(m => m.Map<IntranetRepairReadDto>(It.IsAny<object>())).Returns(expected);

            var result = await _sut.CreateAsync(dto);

            Assert.Equal(expected, result);
        }
    }
}
