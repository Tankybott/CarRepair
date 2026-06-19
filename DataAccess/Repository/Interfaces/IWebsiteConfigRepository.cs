using DataAccess.Repository.IRepository;
using Model.DomainModel;

namespace DataAccess.Repository.Interfaces
{
    public interface IWebsiteConfigRepository : IRepository<WebsiteConfig>
    {
        Task<WebsiteConfig> GetConfigAsync(bool tracked = false);
    }
}
