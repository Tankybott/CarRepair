using Microsoft.AspNetCore.Mvc;
using Model.DTOs.IntranetDto;
using Model.ViewModel;

namespace CarRepair.Areas.Intranet.Controllers
{
    [Area("Intranet")]
    public class CarController : Controller
    {
        public IActionResult Index()
        {
            var vm = new IntranetCarViewModel
            {
                Cars = new List<IntranetCarReadDto>
                {
                    new() { Id = 1, RegistrationNumber = "ABC123", OwnerFullName = "John Doe" },
                    new() { Id = 2, RegistrationNumber = "XYZ987", OwnerFullName = "Anna Kowalski" },
                    new() { Id = 3, RegistrationNumber = "ZZZ555", OwnerFullName = "Mark Smith" }
                }
            };

            return View(vm);
        }
    }
}
