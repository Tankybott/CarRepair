using Microsoft.AspNetCore.Mvc;
using Model.DTOs.PortalDto;
using Model.ViewModel;
using System.Diagnostics;
using ToolShop.Models;

namespace ToolShop.Areas.Portal.Controllers
{
    [Area("Portal")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Cart()
        {
            var vm = new CartViewModel
            {
                Services = new List<CartServiceDto>
                {
                    new CartServiceDto
                    {
                        ServiceName = "Oil Change",
                        ShortDescription = "Replace engine oil and filter",
                        EstimatedPrice = 199
                    },
                    new CartServiceDto
                    {
                        ServiceName = "Brake Inspection",
                        ShortDescription = "Check pads, discs and brake fluid",
                        EstimatedPrice = 149
                    },
                    new CartServiceDto
                    {
                        ServiceName = "Engine Diagnostics",
                        ShortDescription = "Full OBD-II scan and fault analysis",
                        EstimatedPrice = 249
                    }
                }
            };

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
