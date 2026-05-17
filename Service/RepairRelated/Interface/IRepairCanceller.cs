namespace Service.RepairRelated.Interface
{
    public interface IRepairCanceller
    {
        Task CancelAsync(int repairId);
    }
}
