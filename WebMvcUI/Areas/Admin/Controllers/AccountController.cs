using Microsoft.AspNetCore.Mvc;

namespace WebMvcUI.Areas.Admin.Controllers
{
    [Area("admin")]
    public class AccountController : Controller
    {
      
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        public IActionResult Register(string returnUrl)
        {
            return View();
        }
    }
}
