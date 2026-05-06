namespace Service.CarRelated.Interface
{
    public interface ICarDeleter
    {
        Task DeleteAsync(int id);
    }
}
