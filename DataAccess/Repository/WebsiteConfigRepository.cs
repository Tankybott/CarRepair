using DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Model.DomainModel;

namespace DataAccess.Repository
{
    public class WebsiteConfigRepository : Repository<WebsiteConfig>, IWebsiteConfigRepository
    {
        public WebsiteConfigRepository(ApplicationDbContext db) : base(db) { }

        public async Task<WebsiteConfig> GetConfigAsync(bool tracked = false)
        {
            IQueryable<WebsiteConfig> query = tracked ? dbSet : dbSet.AsNoTracking();
            return await query.FirstAsync();
        }

        public override void Remove(WebsiteConfig entity)
        {
            throw new InvalidOperationException("WebsiteConfig cannot be deleted.");
        }

        public override void RemoveRange(IEnumerable<WebsiteConfig> entities)
        {
            throw new InvalidOperationException("WebsiteConfig cannot be deleted.");
        }

        public override void Add(WebsiteConfig entity)
        {
            throw new InvalidOperationException("WebsiteConfig cannot be Added.");
        }
    }
}
