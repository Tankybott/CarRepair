using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DTOs.IntranetDto;
using Model.DTOs.PortalDto;
using Moq;
using Service.ServiceRelated;
using System.Linq.Expressions;

namespace CarRepairTest.ServiceRelated
{
    public class ServiceReaderTests
    {
        private readonly Mock<IServiceRepository> _repoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly ServiceReadService _sut;

        public ServiceReaderTests()
        {
            _sut = new ServiceReadService(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllForIndex_ShouldReturnMappedDtos_WhenServicesExist()
        {
            var services = new List<Model.DomainModel.Service> { new() { Id = 1 } };
            var expected = new List<IntranetServiceReadDto> { new IntranetServiceReadDto { Id = 1 } };
            _repoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Model.DomainModel.Service, object>>[]>()))
                .ReturnsAsync(services);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<Model.DomainModel.Service>, IEnumerable<IntranetServiceReadDto>>(services))
                .Returns(expected);

            var result = await _sut.GetAllForIndex();

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetAllForIndex_ShouldReturnEmptyCollection_WhenNoServicesExist()
        {
            var services = new List<Model.DomainModel.Service>();
            _repoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Model.DomainModel.Service, object>>[]>()))
                .ReturnsAsync(services);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<Model.DomainModel.Service>, IEnumerable<IntranetServiceReadDto>>(services))
                .Returns(new List<IntranetServiceReadDto>());

            var result = await _sut.GetAllForIndex();

            Assert.Empty(result);
        }
    }

    public class PortalServiceReaderTests
    {
        private readonly Mock<IServiceRepository> _repoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly PortalServiceReaderService _sut;

        public PortalServiceReaderTests()
        {
            _sut = new PortalServiceReaderService(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetForBasket_ShouldReturnNull_WhenServiceNotFound()
        {
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Model.DomainModel.Service, object>>[]>()))
                .ReturnsAsync((Model.DomainModel.Service?)null);

            var result = await _sut.GetForBasket(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetForBasket_ShouldReturnMappedDto_WhenServiceExists()
        {
            var service = new Model.DomainModel.Service { Id = 1 };
            var expected = new CartServiceDto { Id = 1 };
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Model.DomainModel.Service, object>>[]>()))
                .ReturnsAsync(service);
            _mapperMock
                .Setup(m => m.Map<Model.DomainModel.Service, CartServiceDto>(service))
                .Returns(expected);

            var result = await _sut.GetForBasket(1);

            Assert.Equal(expected, result);
        }
    }
}
