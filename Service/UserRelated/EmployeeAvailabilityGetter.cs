using DataAccess.Repository.Interfaces;
using Model.DTOs.IntranetDto;
using Service.UserRelated.Interface;

namespace Service.UserRelated
{
    public class EmployeeAvailabilityGetter : IEmployeeAvailabilityGetter
    {
        private readonly IEmployeeProfileRepository _employeeProfileRepository;

        public EmployeeAvailabilityGetter(IEmployeeProfileRepository employeeProfileRepository)
        {
            _employeeProfileRepository = employeeProfileRepository;
        }

        public async Task<IEnumerable<IntranetAvailableEmployeeDto>> GetAvailableAsync(int serviceTypeId, DateTime start, DateTime end, int? excludeTaskId = null)
        {
            var employees = await _employeeProfileRepository.GetAvailableForServiceAsync(serviceTypeId, start, end, excludeTaskId);
            return employees.Select(e => new IntranetAvailableEmployeeDto
            {
                Id = e.Id,
                FullName = ((e.Name ?? string.Empty) + " " + (e.Surname ?? string.Empty)).Trim(),
                EmployeeNumber = e.EmployeeNumber
            });
        }
    }
}