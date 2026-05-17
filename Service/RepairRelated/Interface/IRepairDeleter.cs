namespace Service.RepairRelated.Interface
{
    public interface IRepairDeleter
    {
        Task DeleteAsync(int repairId);
    }
}
