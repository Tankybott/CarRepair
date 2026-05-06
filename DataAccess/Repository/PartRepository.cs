using DataAccess.Repository.Interfaces;
using Model.DomainModel;

namespace DataAccess.Repository
{
    public class PartRepository : Repository<Part>, IPartRepository
    {
        public PartRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }
    }
}
