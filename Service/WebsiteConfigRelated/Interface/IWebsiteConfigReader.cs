using Model.DTOs.IntranetDto;

namespace Service.WebsiteConfigRelated.Interface
{
    public interface IWebsiteConfigReader
    {
        Task<WebsiteConfigDto> GetConfigAsync();
    }
}
