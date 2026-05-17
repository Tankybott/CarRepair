using DataAccess.Repository.Interfaces;
using Service.RepairRelated.Interface;

namespace Service.RepairRelated
{
    public class OverheadCostDeleter : IOverheadCostDeleter
    {
        private readonly ICostEstimationItemRepository _repo;

        public OverheadCostDeleter(ICostEstimationItemRepository repo)
        {
            _repo = repo;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _repo.GetAsync(i => i.Id == id, tracked: true);
            if (item != null)
            {
                item.DeletedAt = DateTime.UtcNow;
                await _repo.SaveAsync();
            }
        }
    }
}
