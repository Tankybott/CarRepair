namespace Service.ServiceRelated.Interface
{
    public interface IServiceDeleter
    {
        Task DeleteAsync(int id);
    }
}
