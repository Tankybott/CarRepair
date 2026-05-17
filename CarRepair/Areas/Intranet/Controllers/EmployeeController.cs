using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs.IntranetDto;
using Model.ViewModel;
using Service.ServiceTypeRelated.Interface;
using Service.UserRelated.Interface;

namespace CarRepair.Areas.Intranet.Controllers
{
    [Area("Intranet")]
    [Authorize(Roles = "Admin,Manager")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeCreator _employeeCreator;
        private readonly IEmployeeUpdater _employeeUpdater;
        private readonly IEmployeeDeleter _employeeDeleter;
        private readonly IEmployeeReader _employeeReader;
        private readonly IServiceTypeReader _serviceTypeReader;

        public EmployeeController(IEmployeeCreator employeeCreator, IEmployeeUpdater employeeUpdater, IEmployeeDeleter employeeDeleter, IEmployeeReader employeeReader, IServiceTypeReader serviceTypeReader)
        {
            _employeeCreator = employeeCreator;
            _employeeUpdater = employeeUpdater;
            _employeeDeleter = employeeDeleter;
            _employeeReader = employeeReader;
            _serviceTypeReader = serviceTypeReader;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var employees = await _employeeReader.GetAllForIndex();
                var serviceTypes = await _serviceTypeReader.GetAllForIndex();

                return View(new IntranetEmployeeViewModel
                {
                    Employees = employees,
                    ServiceTypes = serviceTypes
                });
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { area = "Portal" });
            }
        }

        [HttpPost]
        [Route("api/employee/create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] IntranetEmployeeCreateDto dto)
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
                var saved = await _employeeCreator.CreateAsync(dto);
                return Ok(new { success = true, message = "Created successfully.", data = saved });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/employee/update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] IntranetEmployeeUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    error = "Invalid data submitted.",
                    validation = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            var saved = await _employeeUpdater.UpdateAsync(dto);
            return Ok(new { success = true, message = "Updated successfully.", data = saved });
        }

        [HttpDelete]
        [Route("api/employee/delete/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeDeleter.DeleteAsync(id);
            return Ok(new { success = true, message = "Deleted successfully." });
        }
    }
}
