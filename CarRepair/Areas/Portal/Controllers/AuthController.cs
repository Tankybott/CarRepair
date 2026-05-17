using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Model.DTOs.PortalDto;
using Service.UserRelated.Interface;

namespace CarRepair.Areas.Portal.Controllers
{
    [Area("Portal")]
    public class AuthController : Controller
    {
        private readonly IClientCreator _clientCreator;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(IClientCreator clientCreator, SignInManager<ApplicationUser> signInManager)
        {
            _clientCreator = clientCreator;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(IntranetClientCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _clientCreator.CreateAsync(dto);
                await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, isPersistent: false, lockoutOnFailure: false);
                return RedirectToAction("Index", "Home");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(PortalLoginDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, dto.RememberMe, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(dto);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
