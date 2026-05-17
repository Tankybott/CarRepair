using CarRepair.Services;
using Microsoft.AspNetCore.Mvc;
using Model.ViewModel;
using Service.ServiceRelated.Interface;
using Service.ServiceTypeRelated.Interface;

namespace CarRepair.Areas.Portal.Controllers
{
    [Area("Portal")]
    public class OfferController : Controller
    {
        private readonly IOfferReader _offerReader;
        private readonly IPortalServiceReader _portalServiceReader;
        private readonly ICartService _basketService;

        public OfferController(IOfferReader offerReader, IPortalServiceReader portalServiceReader, ICartService basketService)
        {
            _offerReader = offerReader;
            _portalServiceReader = portalServiceReader;
            _basketService = basketService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _offerReader.GetAllForOffer();
            return View(new OfferViewModel { Categories = categories });
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket(int id)
        {
            var service = await _portalServiceReader.GetForBasket(id);
            if (service is not null)
                _basketService.AddService(HttpContext.Session, service);

            TempData["Added"] = service?.ServiceName;
            return RedirectToAction(nameof(Index));
        }
    }
}
