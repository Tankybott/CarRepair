using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.PartRelated;
using System.Linq.Expressions;

namespace CarRepairTest.PartRelated
{
    public class PartReaderTests
    {
        private readonly Mock<IPartRepository> _partRepositoryMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly PartReadService _sut;

        public PartReaderTests()
        {
            _sut = new PartReadService(_partRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllForIndex_ShouldReturnMappedDtos_WhenPartsExist()
        {
            var parts = new List<Part> { new Part { Id = 1, Name = "Brake Pad" } };
            var expected = new List<IntranetPartReadDto> { new IntranetPartReadDto { Id = 1, Name = "Brake Pad" } };
            _partRepositoryMock
                .Setup(r => r.GetAllAsync(
                    It.IsAny<Expression<Func<Part, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Part, object>>[]>()))
                .ReturnsAsync(parts);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<Part>, IEnumerable<IntranetPartReadDto>>(parts))
                .Returns(expected);

            var result = await _sut.GetAllForIndex();

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetAllForIndex_ShouldReturnEmptyCollection_WhenNoPartsExist()
        {
            var parts = new List<Part>();
            _partRepositoryMock
                .Setup(r => r.GetAllAsync(
                    It.IsAny<Expression<Func<Part, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<Part, object>>[]>()))
                .ReturnsAsync(parts);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<Part>, IEnumerable<IntranetPartReadDto>>(parts))
                .Returns(new List<IntranetPartReadDto>());

            var result = await _sut.GetAllForIndex();

            Assert.Empty(result);
        }
    }
}
