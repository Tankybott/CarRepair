using Microsoft.AspNetCore.Mvc;
using Model.DTOs.IntranetDto;
using Model.ViewModel;
using Service.ServiceRelated.Interface;
using Service.ServiceTypeRelated.Interface;

namespace CarRepair.Areas.Intranet.Controllers
{
    [Area("Intranet")]
    public class ServiceController : Controller
    {
        private readonly IServiceBase _serviceBase;
        private readonly IServiceReader _serviceReader;
        private readonly IServiceTypeReader _serviceTypeReader;

        public ServiceController(IServiceBase serviceBase, IServiceReader serviceReader, IServiceTypeReader serviceTypeReader)
        {
            _serviceBase = serviceBase;
            _serviceReader = serviceReader;
            _serviceTypeReader = serviceTypeReader;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var services = await _serviceReader.GetAllForIndex();
                var serviceTypes = await _serviceTypeReader.GetAllForIndex();

                var vm = new IntranetServiceIndexVM
                {
                    Items = services,
                    ServiceTypes = serviceTypes
                };

                return View(vm);
            }
            catch
            {
                TempData["Error"] = "Failed to load services.";
                return View(new IntranetServiceIndexVM());
            }
        }

        [HttpPost]
        [Route("api/service/upsert")]
        public async Task<IActionResult> Upsert([FromBody] IntranetServiceUpsertDto dto)
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
                var saved = await _serviceBase.Upsert(dto);

                return Ok(new
                {
                    success = true,
                    message = dto.Id == 0 ? "Created successfully." : "Updated successfully.",
                    data = new
                    {
                        Id = saved.Id,
                        ServiceName = saved.ServiceName,
                        ServiceType = saved.ServiceType,
                        ServiceTypeId = saved.ServiceTypeId,
                        Description = saved.Description,
                        AveragePrice = saved.AveragePrice
                    }
                });
            }
            catch
            {
                return StatusCode(500, new { success = false, error = "Upsert failed." });
            }
        }

        [HttpDelete]
        [Route("api/service/delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _serviceBase.DeleteAsync(id);
                return Ok(new { success = true, message = "Deleted successfully." });
            }
            catch
            {
                return StatusCode(500, new { success = false, error = "Delete failed." });
            }
        }
    }
}
