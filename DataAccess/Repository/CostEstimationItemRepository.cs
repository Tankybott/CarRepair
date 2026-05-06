using DataAccess.Repository.Interfaces;
using Model.DomainModel;

namespace DataAccess.Repository
{
    public class CostEstimationItemRepository : Repository<CostEstimationItem>, ICostEstimationItemRepository
    {
        public CostEstimationItemRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }
    }
}
