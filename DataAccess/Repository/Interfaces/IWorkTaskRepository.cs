using DataAccess.Repository.IRepository;
using Model.DomainModel;

namespace DataAccess.Repository.Interfaces
{
    public interface IWorkTaskRepository : IRepository<WorkTask>
    {
        Task<IEnumerable<WorkTask>> GetAllForRepairAsync(int repairId);
        Task<WorkTask?> GetTrackedWithEmployeesAsync(int id);
    }
}
