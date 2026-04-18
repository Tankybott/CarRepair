using Microsoft.AspNetCore.Mvc;
using Model.DTOs.IntranetDto;
using Model.ViewModel;

namespace CarRepair.Areas.Intranet.Controllers
{
    [Area("Intranet")]
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            var vm = new IntranetClientViewModel
            {
                Clients = new List<IntranetClientReadDto>
                {
                    new() { Id = 1, FullName = "John Doe", PhoneNumber = "123456789" },
                    new() { Id = 2, FullName = "Anna Kowalski", PhoneNumber = "987654321" },
                    new() { Id = 3, FullName = "Mark Smith", PhoneNumber = "555111222" }
                }
            };

            return View(vm);
        }
    }
}
