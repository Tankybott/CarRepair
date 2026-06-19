using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Moq;
using Service.UserRelated;
using System.Linq.Expressions;

namespace CarRepairTest.UserRelated
{
    public class EmployeeReaderTests
    {
        private readonly Mock<IEmployeeProfileRepository> _employeeRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly EmployeeReadService _sut;

        public EmployeeReaderTests()
        {
            _sut = new EmployeeReadService(_employeeRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllForIndex_ShouldReturnMappedDtos_WhenEmployeesExist()
        {
            var employees = new List<EmployeeProfile> { new() { Id = 1 } };
            var expected = new List<IntranetEmployeeReadDto> { new() { Id = 1 } };
            _employeeRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<EmployeeProfile, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<EmployeeProfile, object>>[]>()))
                .ReturnsAsync(employees);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<EmployeeProfile>, IEnumerable<IntranetEmployeeReadDto>>(employees))
                .Returns(expected);

            var result = await _sut.GetAllForIndex();

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetAllForIndex_ShouldReturnEmptyCollection_WhenNoEmployeesExist()
        {
            var employees = new List<EmployeeProfile>();
            _employeeRepoMock
                .Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<EmployeeProfile, bool>>>(), It.IsAny<bool>(), It.IsAny<Expression<Func<EmployeeProfile, object>>[]>()))
                .ReturnsAsync(employees);
            _mapperMock
                .Setup(m => m.Map<IEnumerable<EmployeeProfile>, IEnumerable<IntranetEmployeeReadDto>>(employees))
                .Returns(new List<IntranetEmployeeReadDto>());

            var result = await _sut.GetAllForIndex();

            Assert.Empty(result);
        }
    }
}
