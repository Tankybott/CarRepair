using Model.DomainModel;

namespace Service.RepairRelated.Interface
{
    public interface IRepairStatusUpdater
    {
        Task UpdateStatusAsync(int repairId, RepairStatus status);
    }
}
