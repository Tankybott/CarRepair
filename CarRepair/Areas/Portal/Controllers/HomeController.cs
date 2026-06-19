using CarRepair.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Model.DTOs.PortalDto;
using Model.ViewModel;
using Service.CarRelated.Interface;
using Service.RepairRelated.Interface;
using Service.WebsiteConfigRelated.Interface;
using System.Diagnostics;
using ToolShop.Models;

namespace ToolShop.Areas.Portal.Controllers
{
    [Area("Portal")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICartService _basketService;
        private readonly ICarReader _carReader;
        private readonly IRepairCreator _repairCreator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebsiteConfigReader _configReader;

        public HomeController(
            ILogger<HomeController> logger,
            ICartService basketService,
            ICarReader carReader,
            IRepairCreator repairCreator,
            UserManager<ApplicationUser> userManager,
            IWebsiteConfigReader configReader)
        {
            _logger = logger;
            _basketService = basketService;
            _carReader = carReader;
            _repairCreator = repairCreator;
            _userManager = userManager;
            _configReader = configReader;
        }

        public async Task<IActionResult> Index()
        {
            var config = await _configReader.GetConfigAsync();
            return View(config);
        }

        public async Task<IActionResult> Cart()
        {
            var services = _basketService.GetCart(HttpContext.Session);
            var cars = new List<PortalCarDto>();

            var userId = _userManager.GetUserId(User);
            if (userId is not null)
                cars = (await _carReader.GetCarsForUser(userId)).ToList();

            return View(new CartViewModel
            {
                Services = services,
                Cars = cars
            });
        }

        [HttpPost]
        public IActionResult RemoveFromBasket(int serviceId)
        {
            _basketService.RemoveService(HttpContext.Session, serviceId);
            return RedirectToAction(nameof(Cart));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateRepair(int carId, string clientDescription)
        {
            var services = _basketService.GetCart(HttpContext.Session);
            if (!services.Any())
                return RedirectToAction(nameof(Cart));

            var dto = new IntranetRepairCreateDto
            {
                CarId = carId,
                ClientDescription = clientDescription ?? string.Empty,
                ServiceIds = services.Select(s => s.Id).ToList()
            };

            await _repairCreator.CreateAsync(dto);
            _basketService.Clear(HttpContext.Session);

            TempData["RepairCreated"] = true;
            return RedirectToAction("Index", "CarAndRepair", new { area = "Portal" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
