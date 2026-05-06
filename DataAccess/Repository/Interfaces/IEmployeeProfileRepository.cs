using DataAccess.Repository.IRepository;
using Model.DomainModel;

namespace DataAccess.Repository.Interfaces
{
    public interface IEmployeeProfileRepository : IRepository<EmployeeProfile>
    {
        Task<IEnumerable<EmployeeProfile>> GetAvailableForServiceAsync(int serviceTypeId, DateTime start, DateTime end, int? excludeTaskId = null);
    }
}
