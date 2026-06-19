using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.PartRelated;
using System.Linq.Expressions;

namespace CarRepairTest.PartRelated
{
    public class PartCreatorTests
    {
        private readonly Mock<IPartRepository> _partRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly PartCreator _sut;

        public PartCreatorTests()
        {
            _sut = new PartCreator(_partRepositoryMock.Object, _mapperMock.Object);
        }

        private void SetupDefaultMocks(IntranetPartUpsertDto dto, Part part, IntranetPartReadDto readDto)
        {
            _mapperMock.Setup(m => m.Map<Part>(dto)).Returns(part);
            _mapperMock.Setup(m => m.Map<IntranetPartReadDto>(part)).Returns(readDto);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddEntityToRepository_WhenDtoIsValid()
        {
            var dto = new IntranetPartUpsertDto { Name = "Oil Filter" };
            var part = new Part { Id = 1 };
            SetupDefaultMocks(dto, part, new IntranetPartReadDto());

            await _sut.CreateAsync(dto);

            _partRepositoryMock.Verify(r => r.Add(part), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldCallSaveAsync_WhenDtoIsValid()
        {
            var dto = new IntranetPartUpsertDto { Name = "Oil Filter" };
            var part = new Part { Id = 1 };
            SetupDefaultMocks(dto, part, new IntranetPartReadDto());

            await _sut.CreateAsync(dto);

            _partRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnMappedReadDto_WhenPartIsCreated()
        {
            var dto = new IntranetPartUpsertDto { Name = "Oil Filter" };
            var part = new Part { Id = 1 };
            var expected = new IntranetPartReadDto { Id = 1, Name = "Oil Filter" };
            SetupDefaultMocks(dto, part, expected);

            var result = await _sut.CreateAsync(dto);

            Assert.Equal(expected, result);
        }
    }
}
