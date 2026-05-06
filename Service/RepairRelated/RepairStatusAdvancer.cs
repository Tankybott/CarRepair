using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Service.RepairRelated.Interface;

namespace Service.RepairRelated
{
    public class RepairStatusUpdater : IRepairStatusUpdater
    {
        private readonly IRepairRepository _repairRepository;

        public RepairStatusUpdater(IRepairRepository repairRepository)
        {
            _repairRepository = repairRepository;
        }

        public async Task UpdateStatusAsync(int repairId, RepairStatus status)
        {
            var repair = await _repairRepository.GetAsync(r => r.Id == repairId, tracked: true);
            if (repair == null) return;
            repair.Status = status;
            await _repairRepository.SaveAsync();
        }
    }
}
