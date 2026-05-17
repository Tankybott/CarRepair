namespace Service.RepairRelated.Interface
{
    public interface IOverheadCostDeleter
    {
        Task DeleteAsync(int id);
    }
}
