using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DTOs.IntranetDto;
using Moq;
using Service.ServiceRelated;
using System.Linq.Expressions;

namespace CarRepairTest.ServiceRelated
{
    public class ServiceCreatorTests
    {
        private readonly Mock<IServiceRepository> _repoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly ServiceCreator _sut;

        public ServiceCreatorTests()
        {
            _sut = new ServiceCreator(_repoMock.Object, _mapperMock.Object);
        }

        private void SetupDefaultMocks(IntranetServiceUpsertDto dto, Model.DomainModel.Service entity, IntranetServiceReadDto readDto)
        {
            _mapperMock.Setup(m => m.Map<Model.DomainModel.Service>(dto)).Returns(entity);
            _repoMock
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<Model.DomainModel.Service, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<Model.DomainModel.Service, object>>[]>()))
                .ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<IntranetServiceReadDto>(It.IsAny<object>())).Returns(readDto);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddEntityToRepository_WhenDtoIsValid()
        {
            var dto = new IntranetServiceUpsertDto { ShortDescription = "Oil Change" };
            var entity = new Model.DomainModel.Service { Id = 1 };
            SetupDefaultMocks(dto, entity, new IntranetServiceReadDto());

            await _sut.CreateAsync(dto);

            _repoMock.Verify(r => r.Add(entity), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCallSaveAsync_WhenDtoIsValid()
        {
            var dto = new IntranetServiceUpsertDto { ShortDescription = "Oil Change" };
            var entity = new Model.DomainModel.Service { Id = 1 };
            SetupDefaultMocks(dto, entity, new IntranetServiceReadDto());

            await _sut.CreateAsync(dto);

            _repoMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnMappedReadDto_WhenServiceIsCreated()
        {
            var dto = new IntranetServiceUpsertDto { ShortDescription = "Oil Change" };
            var entity = new Model.DomainModel.Service { Id = 1 };
            var expected = new IntranetServiceReadDto { Id = 1, ServiceName = "Oil Change" };
            SetupDefaultMocks(dto, entity, expected);

            var result = await _sut.CreateAsync(dto);

            Assert.Equal(expected, result);
        }
    }
}
