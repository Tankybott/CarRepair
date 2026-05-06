using DataAccess.Repository.Interfaces;
using Model.DomainModel;

namespace DataAccess.Repository
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        public CarRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }
    }
}
