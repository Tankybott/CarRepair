namespace Service.UserRelated.Interface
{
    public interface IClientDeleter
    {
        Task DeleteAsync(int clientProfileId);
    }
}
