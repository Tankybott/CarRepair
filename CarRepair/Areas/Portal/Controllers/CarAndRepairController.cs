using Microsoft.AspNetCore.Mvc;
using Model.DTOs.PortalDto;
using Model.ViewModel;

namespace CarRepair.Areas.Portal.Controllers
{
    [Area("Portal")]
    public class CarAndRepairController : Controller
    {
        public IActionResult Index()
        {
            var vm = new CarAndRepairViewModel
            {
                Cars = new List<CarAndRepairCarDto>
                {
                    new CarAndRepairCarDto { Id = 1, Brand = "BMW", Model = "E90 320d", VIN = "WBAVB123456789", Year = 2008 },
                    new CarAndRepairCarDto { Id = 2, Brand = "Audi", Model = "A4 B8", VIN = "WAUZZZ987654321", Year = 2012 },
                    new CarAndRepairCarDto { Id = 3, Brand = "Volkswagen", Model = "Golf 7", VIN = "WVWZZZ123987654", Year = 2016 }
                }
            };

            return View(vm);
        }
    }
}
