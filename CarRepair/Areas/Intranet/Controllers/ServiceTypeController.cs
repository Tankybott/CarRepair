using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs.IntranetDto;
using Model.ViewModel;
using Service.ServiceTypeRelated.Interface;

namespace CarRepair.Areas.Intranet.Controllers
{
    [Area("Intranet")]
    [Authorize(Roles = "Admin,Manager")]
    public class ServiceTypeController : Controller
    {
        private readonly IServiceTypeCreator _serviceTypeCreator;
        private readonly IServiceTypeUpdater _serviceTypeUpdater;
        private readonly IServiceTypeDeleter _serviceTypeDeleter;
        private readonly IServiceTypeReader _serviceTypeReader;

        public ServiceTypeController(IServiceTypeCreator serviceTypeCreator, IServiceTypeUpdater serviceTypeUpdater, IServiceTypeDeleter serviceTypeDeleter, IServiceTypeReader serviceTypeReader)
        {
            _serviceTypeCreator = serviceTypeCreator;
            _serviceTypeUpdater = serviceTypeUpdater;
            _serviceTypeDeleter = serviceTypeDeleter;
            _serviceTypeReader = serviceTypeReader;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var serviceTypes = await _serviceTypeReader.GetAllForIndex();
                return View(new ServiceTypeIndexVM { Items = serviceTypes });
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { area = "Portal" });
            }
        }

        [HttpPost]
        [Route("api/servicetype/upsert")]
        public async Task<IActionResult> Upsert([FromBody] ServiceTypeDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    error = "Invalid data submitted.",
                    validation = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            var saved = dto.Id == 0
                ? await _serviceTypeCreator.CreateAsync(dto)
                : await _serviceTypeUpdater.UpdateAsync(dto);

            return Ok(new
            {
                success = true,
                message = dto.Id == 0 ? "Created successfully." : "Updated successfully.",
                data = saved
            });
        }

        [HttpDelete]
        [Route("api/servicetype/delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _serviceTypeDeleter.DeleteAsync(id);
                return Ok(new { success = true, message = "Deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }
    }
}
