using DataAccess.Repository.Interfaces;
using Service.PartRelated.Interface;

namespace Service.PartRelated
{
    public class PartDeleter : IPartDeleter
    {
        private readonly IPartRepository _partRepository;

        public PartDeleter(IPartRepository partRepository)
        {
            _partRepository = partRepository;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _partRepository.GetAsync(p => p.Id == id, tracked: true);

            if (entity == null)
                throw new Exception($"Part with id {id} not found.");

            entity.DeletedAt = DateTime.UtcNow;
            await _partRepository.SaveAsync();
        }
    }
}
