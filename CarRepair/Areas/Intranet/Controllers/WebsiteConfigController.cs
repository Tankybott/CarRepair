using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs.IntranetDto;
using Service.WebsiteConfigRelated.Interface;

namespace CarRepair.Areas.Intranet.Controllers
{
    [Area("Intranet")]
    [Authorize(Roles = "Admin,Manager")]
    public class WebsiteConfigController : Controller
    {
        private readonly IWebsiteConfigReader _reader;
        private readonly IWebsiteConfigUpdater _updater;

        public WebsiteConfigController(IWebsiteConfigReader reader, IWebsiteConfigUpdater updater)
        {
            _reader = reader;
            _updater = updater;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var config = await _reader.GetConfigAsync();
                return View(config);
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { area = "Portal" });
            }
        }

        [HttpPost]
        [Route("api/websiteconfig/update")]
        public async Task<IActionResult> Update([FromBody] WebsiteConfigDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    error = "Invalid data submitted.",
                    validation = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            try
            {
                var saved = await _updater.UpdateAsync(dto);
                return Ok(new { success = true, message = "Configuration saved.", data = saved });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
}
