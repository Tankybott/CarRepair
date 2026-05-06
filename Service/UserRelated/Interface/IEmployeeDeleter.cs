namespace Service.UserRelated.Interface
{
    public interface IEmployeeDeleter
    {
        Task DeleteAsync(int id);
    }
}
