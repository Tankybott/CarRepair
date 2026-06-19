using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Model.DTOs.PortalDto;
using Moq;
using Service.ServiceTypeRelated;
using System.Linq.Expressions;

namespace CarRepairTest.ServiceTypeRelated
{
    public class ServiceTypeReaderTests
    {
        private readonly Mock<IServiceTypeRepository> _repoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly ServiceTypeReadService _sut;

        public ServiceTypeReaderTests()
        {
            _sut = new ServiceTypeReadService(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllForIndex_ShouldReturnMappedDtos_WhenServiceTypesExist()
        {
            var entities = new List<ServiceType> { new ServiceType { Id = 1, Name = "Engine" } };
            var expected = new List<ServiceTypeDto> { new ServiceTypeDto { Id = 1, Name = "Engine" } };
            _repoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()))
                .ReturnsAsync(entities);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<ServiceType>, IEnumerable<ServiceTypeDto>>(entities))
                .Returns(expected);

            var result = await _sut.GetAllForIndex();

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetAllForIndex_ShouldReturnEmptyCollection_WhenNoServiceTypesExist()
        {
            var entities = new List<ServiceType>();
            _repoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()))
                .ReturnsAsync(entities);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<ServiceType>, IEnumerable<ServiceTypeDto>>(entities))
                .Returns(new List<ServiceTypeDto>());

            var result = await _sut.GetAllForIndex();

            Assert.Empty(result);
        }
    }

    public class OfferReaderTests
    {
        private readonly Mock<IServiceTypeRepository> _repoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly OfferReaderService _sut;

        public OfferReaderTests()
        {
            _sut = new OfferReaderService(_repoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllForOffer_ShouldReturnMappedDtos_WhenServiceTypesExist()
        {
            var entities = new List<ServiceType> { new ServiceType { Id = 1, Name = "Engine" } };
            var expected = new List<OfferServiceTypeDto> { new OfferServiceTypeDto { Name = "Engine" } };
            _repoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()))
                .ReturnsAsync(entities);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<ServiceType>, IEnumerable<OfferServiceTypeDto>>(entities))
                .Returns(expected);

            var result = await _sut.GetAllForOffer();

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetAllForOffer_ShouldReturnEmptyCollection_WhenNoServiceTypesExist()
        {
            var entities = new List<ServiceType>();
            _repoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<ServiceType, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<ServiceType, object>>[]>()))
                .ReturnsAsync(entities);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<ServiceType>, IEnumerable<OfferServiceTypeDto>>(entities))
                .Returns(new List<OfferServiceTypeDto>());

            var result = await _sut.GetAllForOffer();

            Assert.Empty(result);
        }
    }
}
