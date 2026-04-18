using Microsoft.AspNetCore.Mvc;
using Model.DTOs;
using Model.DTOs.IntranetDto;
using Model.ViewModel;

namespace CarRepair.Areas.Intranet.Controllers
{
    [Area("Intranet")]
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            var dummyTypes = new List<string>
            {
                "Engine",
                "Electrical",
                "Bodywork",
                "Inspection"
            };

            var vm = new IntranetServiceIndexVM
            {
                ServiceTypes = dummyTypes,

                Items = new List<IntranetServiceReadDto>
                {
                    new() { Id = 1, ServiceName = "Oil Change", ServiceType = "Engine" },
                    new() { Id = 2, ServiceName = "Battery Replacement", ServiceType = "Electrical" },
                    new() { Id = 3, ServiceName = "Dent Repair", ServiceType = "Bodywork" },
                    new() { Id = 4, ServiceName = "Annual Inspection", ServiceType = "Inspection" }
                }
            };

            return View(vm);
        }
    }
}
