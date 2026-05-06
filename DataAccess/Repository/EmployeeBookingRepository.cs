using DataAccess.Repository.Interfaces;
using Model.DomainModel;

namespace DataAccess.Repository
{
    public class EmployeeBookingRepository : Repository<EmployeeBooking>, IEmployeeBookingRepository
    {
        public EmployeeBookingRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }
    }
}
