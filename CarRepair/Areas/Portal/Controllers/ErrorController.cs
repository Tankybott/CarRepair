using Microsoft.AspNetCore.Mvc;

namespace CarRepair.Areas.Portal.Controllers
{
    [Area("Portal")]
    public class ErrorController : Controller
    {
        [Route("Portal/Error")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
