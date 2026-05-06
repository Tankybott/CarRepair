using DataAccess.Repository.IRepository;
using Model.DomainModel;

namespace DataAccess.Repository.Interfaces
{
    public interface IRepairRepository : IRepository<Repair>
    {
        Task<IEnumerable<Repair>> GetAllWithDetailsAsync();
        Task<Repair?> GetWithDetailsAsync(int id);
        Task<Repair?> GetForManageAsync(int id);
        Task<Repair?> GetTrackedWithServicesAsync(int id);
    }
}
