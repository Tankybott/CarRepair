using DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.DomainModel;

namespace DataAccess.Repository
{
    public class EmployeeProfileRepository : Repository<EmployeeProfile>, IEmployeeProfileRepository
    {
        public EmployeeProfileRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }

        public async Task<IEnumerable<EmployeeProfile>> GetAvailableForServiceAsync(int serviceTypeId, DateTime start, DateTime end, int? excludeTaskId = null)
        {
            return await _db.EmployeeProfiles
                .Where(e => e.DeletedAt == null)
                .Where(e => e.Specializations.Any(s => s.Id == serviceTypeId))
                .Where(e => !e.EmployeeBookings.Any(b =>
                    b.DeletedAt == null &&
                    (excludeTaskId == null || b.TaskId != excludeTaskId) &&
                    b.StartDateTime < end &&
                    b.EndDateTime > start))
                .Include(e => e.ApplicationUser)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
