using DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.DomainModel;

namespace DataAccess.Repository
{
    public class RepairRepository : Repository<Repair>, IRepairRepository
    {
        public RepairRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }

        public async Task<IEnumerable<Repair>> GetAllWithDetailsAsync()
        {
            return await _db.Repairs
                .Where(r => r.DeletedAt == null)
                .Include(r => r.Car)
                    .ThenInclude(c => c.Client)
                .Include(r => r.ServicesSelected)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Repair>> GetAllForCarAndUserAsync(int carId, string userId)
        {
            return await _db.Repairs
                .Where(r => r.CarId == carId && r.DeletedAt == null && r.Car.Client.ApplicationUserId == userId)
                .Include(r => r.Car)
                    .ThenInclude(c => c.Client)
                .Include(r => r.ServicesSelected)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Repair?> GetWithDetailsAsync(int id)
        {
            return await _db.Repairs
                .Where(r => r.Id == id && r.DeletedAt == null)
                .Include(r => r.Car)
                    .ThenInclude(c => c.Client)
                .Include(r => r.ServicesSelected)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Repair?> GetForManageAsync(int id)
        {
            return await _db.Repairs
                .Where(r => r.Id == id && r.DeletedAt == null)
                .Include(r => r.Car)
                    .ThenInclude(c => c.Client)
                        .ThenInclude(cp => cp.ApplicationUser)
                .Include(r => r.ServicesSelected)
                    .ThenInclude(s => s.ServiceType)
                .Include(r => r.PartsUsed)
                .Include(r => r.CostEstimationCollection)
                .Include(r => r.Booking)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Repair?> GetTrackedWithServicesAsync(int id)
        {
            return await _db.Repairs
                .Where(r => r.Id == id && r.DeletedAt == null)
                .Include(r => r.ServicesSelected)
                .FirstOrDefaultAsync();
        }
    }
}
