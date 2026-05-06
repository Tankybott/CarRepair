using Microsoft.AspNetCore.Mvc;
using Model.DTOs.IntranetDto;
using Model.ViewModel;
using Service.ServiceTypeRelated.Interface;

namespace CarRepair.Areas.Intranet.Controllers
{
    [Area("Intranet")]
    public class ServiceTypeController : Controller
    {
        private readonly IServiceTypeBase _serviceTypeBase;
        private readonly IServiceTypeReader _serviceTypeReader;

        public ServiceTypeController(IServiceTypeBase serviceTypeCRUD, IServiceTypeReader serviceTypeReader)
        {
            _serviceTypeBase = serviceTypeCRUD;
            _serviceTypeReader = serviceTypeReader;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var serviceTypes = await _serviceTypeReader.GetAllForIndex();
                var vm = new ServiceTypeIndexVM
                {
                    Items = serviceTypes
                };
                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to load service types.";
                return View(new ServiceTypeIndexVM { Items = Enumerable.Empty<ServiceTypeDto>() });
            }
        }

        [HttpPost]
        [Route("api/servicetype/upsert")]
        public async Task<IActionResult> Upsert([FromBody] ServiceTypeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    error = "Invalid data submitted.",
                    validation = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });
            }

            try
            {
                var saved = await _serviceTypeBase.Upsert(dto);

                return Ok(new
                {
                    success = true,
                    message = dto.Id == 0 ? "Created successfully." : "Updated successfully.",
                    data = new
                    {
                        Id = saved.Id,
                        Name = saved.Name,
                        Description = saved.Description
                    }
                });
            }
            catch
            {
                return StatusCode(500, new
                {
                    success = false,
                    error = "Upsert failed."
                });
            }
        }

        [HttpDelete]
        [Route("api/servicetype/delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _serviceTypeBase.DeleteAsync(id);
                return Ok(new { success = true, message = "Deleted successfully." });
            }
            catch
            {
                return StatusCode(500, new { success = false, error = "Delete failed." });
            }
        }
    }
}
