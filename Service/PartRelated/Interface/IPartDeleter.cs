namespace Service.PartRelated.Interface
{
    public interface IPartDeleter
    {
        Task DeleteAsync(int id);
    }
}
