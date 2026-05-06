using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Model.ViewModel;
using Service.RepairRelated.Interface;
using Service.ServiceRelated.Interface;
using Service.UserRelated.Interface;
using Service.WorkTaskRelated.Interface;

namespace CarRepair.Areas.Intranet.Controllers
{
    [Area("Intranet")]
    public class RepairController : Controller
    {
        private readonly IRepairCreator _repairCreator;
        private readonly IRepairReader _repairReader;
        private readonly IClientReader _clientReader;
        private readonly IServiceReader _serviceReader;
        private readonly ICostEstimationCreator _costEstimationCreator;
        private readonly IRepairServiceAdder _repairServiceAdder;
        private readonly IRepairStatusUpdater _repairStatusUpdater;
        private readonly IRepairScheduler _repairScheduler;
        private readonly IWorkTaskCreator _workTaskCreator;
        private readonly IWorkTaskUpdater _workTaskUpdater;
        private readonly IWorkTaskDeleter _workTaskDeleter;
        private readonly IEmployeeAvailabilityGetter _employeeTimeBooker;
        private readonly DataAccess.Repository.Interfaces.ICarRepository _carRepository;
        private readonly DataAccess.Repository.Interfaces.IRepairRepository _repairRepository;
        private readonly DataAccess.Repository.Interfaces.IWorkTaskRepository _workTaskRepository;
        private readonly IMapper _mapper;

        public RepairController(
            IRepairCreator repairCreator,
            IRepairReader repairReader,
            IClientReader clientReader,
            IServiceReader serviceReader,
            ICostEstimationCreator costEstimationCreator,
            IRepairServiceAdder repairServiceAdder,
            IRepairStatusUpdater repairStatusUpdater,
            IRepairScheduler repairScheduler,
            IWorkTaskCreator workTaskCreator,
            IWorkTaskUpdater workTaskUpdater,
            IWorkTaskDeleter workTaskDeleter,
            IEmployeeAvailabilityGetter employeeTimeBooker,
            DataAccess.Repository.Interfaces.ICarRepository carRepository,
            DataAccess.Repository.Interfaces.IRepairRepository repairRepository,
            DataAccess.Repository.Interfaces.IWorkTaskRepository workTaskRepository,
            IMapper mapper)
        {
            _repairCreator = repairCreator;
            _repairReader = repairReader;
            _clientReader = clientReader;
            _serviceReader = serviceReader;
            _costEstimationCreator = costEstimationCreator;
            _repairServiceAdder = repairServiceAdder;
            _repairStatusUpdater = repairStatusUpdater;
            _repairScheduler = repairScheduler;
            _workTaskCreator = workTaskCreator;
            _workTaskUpdater = workTaskUpdater;
            _workTaskDeleter = workTaskDeleter;
            _employeeTimeBooker = employeeTimeBooker;
            _carRepository = carRepository;
            _repairRepository = repairRepository;
            _workTaskRepository = workTaskRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var repairs = await _repairReader.GetAllForIndex();
                var clients = await _clientReader.GetAllForIndex();
                var services = await _serviceReader.GetAllForIndex();

                return View(new IntranetRepairViewModel
                {
                    Repairs = repairs,
                    Clients = clients,
                    Services = services
                });
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { area = "Portal" });
            }
        }

        public async Task<IActionResult> Manage(int id)
        {
            try
            {
                var repair = await _repairRepository.GetForManageAsync(id);
                if (repair == null) return NotFound();

                var dto = _mapper.Map<IntranetRepairManageDto>(repair);

                var allServices = await _serviceReader.GetAllForIndex();
                var existingIds = dto.Services.Select(s => s.Id).ToHashSet();
                dto.AllAvailableServices = allServices.Where(s => !existingIds.Contains(s.Id)).ToList();

                return View(dto);
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { area = "Portal" });
            }
        }

        public async Task<IActionResult> EstimateCosts(int id)
        {
            try
            {
                var repair = await _repairRepository.GetForManageAsync(id);
                if (repair == null) return NotFound();
                var dto = _mapper.Map<IntranetRepairManageDto>(repair);
                return View(dto);
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { area = "Portal" });
            }
        }

        public async Task<IActionResult> Tasks(int id)
        {
            try
            {
                var repair = await _repairRepository.GetForManageAsync(id);
                if (repair == null) return NotFound();

                var tasks = await _workTaskRepository.GetAllForRepairAsync(id);
                var taskDtos = _mapper.Map<IEnumerable<IntranetWorkTaskReadDto>>(tasks);
                var serviceDtos = _mapper.Map<IEnumerable<IntranetServiceReadDto>>(repair.ServicesSelected);

                return View(new IntranetRepairTasksViewModel
                {
                    RepairId = id,
                    CarLabel = $"{repair.Car.Brand} {repair.Car.Model} ({repair.Car.Year})",
                    ClientFullName = (repair.Car.Client.Name + " " + repair.Car.Client.Surname).Trim(),
                    Tasks = taskDtos,
                    Services = serviceDtos,
                    ExistingDelivery = repair.Booking?.StartDateTime,
                    ExistingPickup = repair.Booking?.EndDateTime
                });
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { area = "Portal" });
            }
        }

        // ── Repair index AJAX ──────────────────────────────────────────────

        [HttpGet]
        [Route("api/repair/cars-by-client/{clientId:int}")]
        public async Task<IActionResult> GetCarsByClient(int clientId)
        {
            var cars = await _carRepository.GetAllAsync(c => c.ClientId == clientId);
            var result = cars.Select(c => new IntranetCarForRepairDto
            {
                Id = c.Id,
                Label = $"{c.Brand} {c.Model} ({c.Year})"
            });
            return Ok(result);
        }

        [HttpPost]
        [Route("api/repair/create")]
        public async Task<IActionResult> Create([FromBody] IntranetRepairCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    error = "Invalid data submitted.",
                    validation = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            var saved = await _repairCreator.CreateAsync(dto);
            return Ok(new { success = true, message = "Repair created successfully.", data = saved });
        }


        [HttpPost]
        [Route("api/repair/add-services")]
        public async Task<IActionResult> AddServices([FromBody] IntranetRepairAddServicesDto dto)
        {
            await _repairServiceAdder.AddServicesAsync(dto.RepairId, dto.ServiceIds);
            return Ok(new { success = true, message = "Services added successfully." });
        }

        [HttpPost]
        [Route("api/repair/update-status/{id:int}/{statusValue:int}")]
        public async Task<IActionResult> UpdateStatus(int id, int statusValue)
        {
            await _repairStatusUpdater.UpdateStatusAsync(id, (RepairStatus)statusValue);
            return Ok(new { success = true, message = "Status updated." });
        }

        [HttpPost]
        [Route("api/repair/estimate")]
        public async Task<IActionResult> SubmitEstimate([FromBody] IntranetEstimateCostsDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, error = "Invalid data submitted." });

            await _costEstimationCreator.CreateAsync(dto);
            return Ok(new { success = true, message = "Estimate saved successfully." });
        }

        [HttpGet]
        [Route("api/repair/available-employees")]
        public async Task<IActionResult> GetAvailableEmployees(
            [FromQuery] int serviceTypeId,
            [FromQuery] DateTime start,
            [FromQuery] DateTime end,
            [FromQuery] int? excludeTaskId = null)
        {
            var employees = await _employeeTimeBooker.GetAvailableAsync(serviceTypeId, start, end, excludeTaskId);
            return Ok(employees);
        }

        [HttpPost]
        [Route("api/repair/task/create")]
        public async Task<IActionResult> CreateTask([FromBody] IntranetWorkTaskCreateDto dto)
        {
            var saved = await _workTaskCreator.CreateAsync(dto);
            return Ok(new { success = true, message = "Task created.", data = saved });
        }

        [HttpPost]
        [Route("api/repair/task/update")]
        public async Task<IActionResult> UpdateTask([FromBody] IntranetWorkTaskUpdateDto dto)
        {
            var saved = await _workTaskUpdater.UpdateAsync(dto);
            return Ok(new { success = true, message = "Task updated.", data = saved });
        }

        [HttpPost]
        [Route("api/repair/task/cancel/{id:int}")]
        public async Task<IActionResult> CancelTask(int id)
        {
            await _workTaskDeleter.CancelAsync(id);
            return Ok(new { success = true, message = "Task cancelled." });
        }

        [HttpDelete]
        [Route("api/repair/task/delete/{id:int}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _workTaskDeleter.DeleteAsync(id);
            return Ok(new { success = true, message = "Task deleted." });
        }

        [HttpPost]
        [Route("api/repair/schedule")]
        public async Task<IActionResult> Schedule([FromBody] IntranetRepairScheduleDto dto)
        {
            try
            {
                await _repairScheduler.ScheduleAsync(dto);
                return Ok(new { success = true, message = "Repair scheduled." });
            }
            catch (InvalidOperationException ex)
            {
                return Ok(new { success = false, error = ex.Message });
            }
        }
    }
}
