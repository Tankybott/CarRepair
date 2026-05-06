using DataAccess.Repository.Interfaces;
using Service.RepairRelated.Interface;

namespace Service.RepairRelated
{
    public class RepairServiceAdder : IRepairServiceAdder
    {
        private readonly IRepairRepository _repairRepository;
        private readonly IServiceRepository _serviceRepository;

        public RepairServiceAdder(IRepairRepository repairRepository, IServiceRepository serviceRepository)
        {
            _repairRepository = repairRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task AddServicesAsync(int repairId, List<int> serviceIds)
        {
            var repair = await _repairRepository.GetTrackedWithServicesAsync(repairId);
            if (repair == null) return;

            var services = await _serviceRepository.GetAllAsync(s => serviceIds.Contains(s.Id), tracked: true);
            var existingIds = repair.ServicesSelected.Select(s => s.Id).ToHashSet();

            foreach (var svc in services)
                if (!existingIds.Contains(svc.Id))
                    repair.ServicesSelected.Add(svc);

            await _repairRepository.SaveAsync();
        }
    }
}
