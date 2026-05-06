namespace Service.RepairRelated.Interface
{
    public interface IRepairServiceAdder
    {
        Task AddServicesAsync(int repairId, List<int> serviceIds);
    }
}
