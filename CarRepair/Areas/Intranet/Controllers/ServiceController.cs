using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs.IntranetDto;
using Model.ViewModel;
using Service.ServiceRelated.Interface;
using Service.ServiceTypeRelated.Interface;

namespace CarRepair.Areas.Intranet.Controllers
{
    [Area("Intranet")]
    [Authorize(Roles = "Admin,Manager")]
    public class ServiceController : Controller
    {
        private readonly IServiceCreator _serviceCreator;
        private readonly IServiceUpdater _serviceUpdater;
        private readonly IServiceDeleter _serviceDeleter;
        private readonly IServiceReader _serviceReader;
        private readonly IServiceTypeReader _serviceTypeReader;

        public ServiceController(IServiceCreator serviceCreator, IServiceUpdater serviceUpdater, IServiceDeleter serviceDeleter, IServiceReader serviceReader, IServiceTypeReader serviceTypeReader)
        {
            _serviceCreator = serviceCreator;
            _serviceUpdater = serviceUpdater;
            _serviceDeleter = serviceDeleter;
            _serviceReader = serviceReader;
            _serviceTypeReader = serviceTypeReader;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var services = await _serviceReader.GetAllForIndex();
                var serviceTypes = await _serviceTypeReader.GetAllForIndex();

                return View(new IntranetServiceIndexVM
                {
                    Items = services,
                    ServiceTypes = serviceTypes
                });
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { area = "Portal" });
            }
        }

        [HttpPost]
        [Route("api/service/upsert")]
        public async Task<IActionResult> Upsert([FromBody] IntranetServiceUpsertDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    error = "Invalid data submitted.",
                    validation = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            var saved = dto.Id == 0
                ? await _serviceCreator.CreateAsync(dto)
                : await _serviceUpdater.UpdateAsync(dto);

            return Ok(new
            {
                success = true,
                message = dto.Id == 0 ? "Created successfully." : "Updated successfully.",
                data = saved
            });
        }

        [HttpDelete]
        [Route("api/service/delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _serviceDeleter.DeleteAsync(id);
            return Ok(new { success = true, message = "Deleted successfully." });
        }
    }
}
