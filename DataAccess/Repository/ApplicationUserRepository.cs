using DataAccess.Repository.IRepository;
using Model;
using Model.DomainModel;

namespace DataAccess.Repository
{
    public class ApplicationUserRepository: Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext applicationDbContex): base(applicationDbContex) { }


    }
}
