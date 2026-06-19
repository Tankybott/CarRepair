using DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Model.ViewModel;
using Service.ServiceTypeRelated.Interface;
using Service.UserRelated.Interface;
using Service.WebsiteConfigRelated.Interface;

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
        private readonly IEmployeeBookingRepository _bookingRepo;
        private readonly IWebsiteConfigReader _websiteConfigReader;

        public EmployeeController(
            IEmployeeCreator employeeCreator,
            IEmployeeUpdater employeeUpdater,
            IEmployeeDeleter employeeDeleter,
            IEmployeeReader employeeReader,
            IServiceTypeReader serviceTypeReader,
            IEmployeeBookingRepository bookingRepo,
            IWebsiteConfigReader websiteConfigReader)
        {
            _employeeCreator = employeeCreator;
            _employeeUpdater = employeeUpdater;
            _employeeDeleter = employeeDeleter;
            _employeeReader = employeeReader;
            _serviceTypeReader = serviceTypeReader;
            _bookingRepo = bookingRepo;
            _websiteConfigReader = websiteConfigReader;
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

        [HttpGet]
        [Route("api/employee/{id:int}/bookings")]
        public async Task<IActionResult> GetBookings(int id, [FromQuery] string date)
        {
            if (!DateOnly.TryParse(date, out var parsedDate))
                return BadRequest(new { error = "Invalid date." });

            var dayStart = parsedDate.ToDateTime(TimeOnly.MinValue);
            var dayEnd = dayStart.AddDays(1);

            var bookings = await _bookingRepo.GetAllAsync(
                b => b.EmployeeProfileId == id && b.StartDateTime < dayEnd && b.EndDateTime > dayStart,
                tracked: false,
                b => b.Task);

            var config = await _websiteConfigReader.GetConfigAsync();
            var schedule = parsedDate.DayOfWeek switch
            {
                DayOfWeek.Monday => config.MondaySchedule,
                DayOfWeek.Tuesday => config.TuesdaySchedule,
                DayOfWeek.Wednesday => config.WednesdaySchedule,
                DayOfWeek.Thursday => config.ThursdaySchedule,
                DayOfWeek.Friday => config.FridaySchedule,
                DayOfWeek.Saturday => config.SaturdaySchedule,
                DayOfWeek.Sunday => config.SundaySchedule,
                _ => null
            };

            var result = bookings
                .OrderBy(b => b.StartDateTime)
                .Select(b => new
                {
                    start = b.StartDateTime.ToString("HH:mm"),
                    end = b.EndDateTime.ToString("HH:mm"),
                    type = b.BookingType.ToString(),
                    label = b.BookingType == BookingType.Task && b.Task != null
                        ? b.Task.Description
                        : b.BookingType.ToString()
                });

            return Ok(new { schedule, bookings = result });
        }
    }
}
