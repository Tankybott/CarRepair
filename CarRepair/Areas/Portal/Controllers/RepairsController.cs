using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.DomainModel;
using Service.RepairRelated.Interface;

namespace CarRepair.Areas.Portal.Controllers
{
    [Area("Portal")]
    [Authorize]
    public class RepairsController : Controller
    {
        private readonly IRepairReader _repairReader;
        private readonly IRepairStatusUpdater _repairStatusUpdater;
        private readonly UserManager<ApplicationUser> _userManager;

        public RepairsController(
            IRepairReader repairReader,
            IRepairStatusUpdater repairStatusUpdater,
            UserManager<ApplicationUser> userManager)
        {
            _repairReader = repairReader;
            _repairStatusUpdater = repairStatusUpdater;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int carId)
        {
            var userId = _userManager.GetUserId(User)!;
            var repairs = await _repairReader.GetForCarAndUser(carId, userId);
            ViewData["CarId"] = carId;
            return View(repairs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var repair = await _repairReader.GetManageForUser(id, userId);
            if (repair == null) return NotFound();
            return View(repair);
        }

        [HttpPost]
        [Route("api/portal/repair/accept-costs/{id:int}")]
        public async Task<IActionResult> AcceptCosts(int id)
        {
            var userId = _userManager.GetUserId(User)!;
            var repair = await _repairReader.GetManageForUser(id, userId);
            if (repair == null) return Forbid();

            await _repairStatusUpdater.UpdateStatusAsync(id, RepairStatus.ClientAccepted);
            return Ok(new { success = true });
        }
    }
}
