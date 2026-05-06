using DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.DomainModel;

namespace DataAccess.Repository
{
    public class WorkTaskRepository : Repository<WorkTask>, IWorkTaskRepository
    {
        public WorkTaskRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }

        public async Task<IEnumerable<WorkTask>> GetAllForRepairAsync(int repairId)
        {
            return await _db.Tasks
                .Where(t => t.RepairId == repairId && t.DeletedAt == null)
                .Include(t => t.EmployeesAssigned)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<WorkTask?> GetTrackedWithEmployeesAsync(int id)
        {
            return await _db.Tasks
                .Where(t => t.Id == id && t.DeletedAt == null)
                .Include(t => t.EmployeesAssigned)
                .FirstOrDefaultAsync();
        }
    }
}
