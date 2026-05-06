namespace Service.WorkTaskRelated.Interface
{
    public interface IWorkTaskDeleter
    {
        Task DeleteAsync(int id);
        Task CancelAsync(int id);
    }
}
