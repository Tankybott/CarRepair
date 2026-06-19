using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.ServiceTypeRelated;

namespace CarRepairTest.ServiceTypeRelated
{
    public class ServiceTypeCreatorTests
    {
        private readonly Mock<IServiceTypeRepository> _repoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly ServiceTypeCreator _sut;

        public ServiceTypeCreatorTests()
        {
            _sut = new ServiceTypeCreator(_repoMock.Object, _mapperMock.Object);
        }

        private void SetupDefaultMocks(ServiceTypeDto dto, ServiceType entity, ServiceTypeDto readDto)
        {
            _mapperMock.Setup(m => m.Map<ServiceType>(dto)).Returns(entity);
            _mapperMock.Setup(m => m.Map<ServiceTypeDto>(entity)).Returns(readDto);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddEntityToRepository_WhenDtoIsValid()
        {
            var dto = new ServiceTypeDto { Name = "Engine Work" };
            var entity = new ServiceType { Id = 1 };
            SetupDefaultMocks(dto, entity, new ServiceTypeDto());

            await _sut.CreateAsync(dto);

            _repoMock.Verify(r => r.Add(entity), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCallSaveAsync_WhenDtoIsValid()
        {
            var dto = new ServiceTypeDto { Name = "Engine Work" };
            var entity = new ServiceType { Id = 1 };
            SetupDefaultMocks(dto, entity, new ServiceTypeDto());

            await _sut.CreateAsync(dto);

            _repoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnMappedDto_WhenEntityIsCreated()
        {
            var dto = new ServiceTypeDto { Name = "Engine Work" };
            var entity = new ServiceType { Id = 1 };
            var expected = new ServiceTypeDto { Id = 1, Name = "Engine Work" };
            SetupDefaultMocks(dto, entity, expected);

            var result = await _sut.CreateAsync(dto);

            Assert.Equal(expected, result);
        }
    }
}
