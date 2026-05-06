using DataAccess.Repository.Interfaces;
using Model.DomainModel;

namespace DataAccess.Repository
{
    public class ClientProfileRepository : Repository<ClientProfile>, IClientProfileRepository
    {
        public ClientProfileRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }
    }
}
