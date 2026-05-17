using DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Model.DTOs.PortalDto;
using Model.ViewModel;
using Service.CarRelated.Interface;

namespace CarRepair.Areas.Portal.Controllers
{
    [Area("Portal")]
    [Authorize]
    public class CarAndRepairController : Controller
    {
        private readonly ICarReader _carReader;
        private readonly ICarCreator _carCreator;
        private readonly ICarUpdater _carUpdater;
        private readonly ICarDeleter _carDeleter;
        private readonly IClientProfileRepository _clientProfileRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CarAndRepairController(
            ICarReader carReader,
            ICarCreator carCreator,
            ICarUpdater carUpdater,
            ICarDeleter carDeleter,
            IClientProfileRepository clientProfileRepository,
            UserManager<ApplicationUser> userManager)
        {
            _carReader = carReader;
            _carCreator = carCreator;
            _carUpdater = carUpdater;
            _carDeleter = carDeleter;
            _clientProfileRepository = clientProfileRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User)!;
            var cars = await _carReader.GetCarsDetailForUser(userId);
            return View(new CarAndRepairViewModel { Cars = cars });
        }

        [HttpPost]
        [Route("api/portal/car/upsert")]
        public async Task<IActionResult> Upsert([FromBody] PortalCarUpsertDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, error = "Invalid data." });

            var userId = _userManager.GetUserId(User)!;

            try
            {
                if (dto.Id == 0)
                {
                    var clientProfile = await _clientProfileRepository.GetAsync(cp => cp.ApplicationUserId == userId);
                    if (clientProfile is null)
                        return BadRequest(new { success = false, error = "Client profile not found." });

                    var createDto = MapToUpsertDto(dto, clientId: clientProfile.Id);
                    var created = await _carCreator.CreateAsync(createDto);
                    return Ok(new { success = true, data = created });
                }
                else
                {
                    var existing = await _carReader.GetCarForUser(dto.Id, userId);
                    if (existing is null)
                        return Forbid();

                    var updateDto = MapToUpsertDto(dto, clientId: existing.ClientId);
                    var updated = await _carUpdater.UpdateAsync(updateDto);
                    return Ok(new { success = true, data = updated });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
        }

        [HttpDelete]
        [Route("api/portal/car/delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var existing = await _carReader.GetCarForUser(id, userId);
            if (existing is null)
                return Forbid();

            await _carDeleter.DeleteAsync(id);
            return Ok(new { success = true });
        }

        private static IntranetCarUpsertDto MapToUpsertDto(PortalCarUpsertDto dto, int clientId) => new()
        {
            Id = dto.Id,
            ClientId = clientId,
            Brand = dto.Brand,
            Model = dto.Model,
            VIN = dto.VIN,
            EngineCode = dto.EngineCode,
            Year = dto.Year,
            FuelType = dto.FuelType,
            Transmission = dto.Transmission,
            BodyType = dto.BodyType,
            ClientNotes = dto.ClientNotes
        };
    }
}
