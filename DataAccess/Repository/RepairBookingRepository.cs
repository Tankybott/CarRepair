using DataAccess.Repository.Interfaces;
using Model.DomainModel;

namespace DataAccess.Repository
{
    public class RepairBookingRepository : Repository<RepairBooking>, IRepairBookingRepository
    {
        public RepairBookingRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }
    }
}
