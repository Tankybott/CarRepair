namespace Service.WebsiteConfigRelated.Interface
{
    public interface IWorkingHoursValidator
    {
        Task ValidateAsync(DateTime start, DateTime end);
    }
}
