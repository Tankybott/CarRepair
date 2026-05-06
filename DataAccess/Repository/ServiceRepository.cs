using DataAccess.Repository.Interfaces;
using Model.DomainModel;

namespace DataAccess.Repository
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }
    }
}
