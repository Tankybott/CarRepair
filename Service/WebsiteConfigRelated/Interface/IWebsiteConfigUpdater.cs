using Model.DTOs.IntranetDto;

namespace Service.WebsiteConfigRelated.Interface
{
    public interface IWebsiteConfigUpdater
    {
        Task<WebsiteConfigDto> UpdateAsync(WebsiteConfigDto dto);
    }
}
