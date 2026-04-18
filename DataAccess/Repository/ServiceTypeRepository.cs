using DataAccess.Repository.Interfaces;
using DataAccess.Repository.IRepository;
using Model.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ServiceTypeRepository: Repository<ServiceType>, IServiceTypeRepository
    {
        public ServiceTypeRepository(ApplicationDbContext applicationDbContext): base(applicationDbContext) { }

    }
}
