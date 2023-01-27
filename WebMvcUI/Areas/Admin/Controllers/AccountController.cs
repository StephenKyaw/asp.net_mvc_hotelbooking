using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebMvcUI.Areas.Admin.Controllers
{
    [Area("admin")]
    public class AccountController : Controller
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var _loginUser = await _userManager.FindByEmailAsync(model.Email);

                if (_loginUser != null)
                {
                    var _checkPassword = await _userManager.CheckPasswordAsync(_loginUser, model.Password);

                    if (_checkPassword)
                    {
                        await _signInManager.SignInAsync(_loginUser, model.RememberMe);

                        var _isAdminRole = await _userManager.IsInRoleAsync(_loginUser, Constants.ROLE_ADMINISTRATORS);

                        if (_isAdminRole)
                        {
                            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                        }else
                        {
                            return RedirectToAction("Index","Home", new { area = "" });
                        }
                    }
                }

            }

            return RedirectToAction("login", "account", new { area = "admin" });
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.Email = model.Email;
                user.UserName = model.Email;

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Constants.ROLE_USER);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }

            }

            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account", new { area = "Admin" });
        }
        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'");
            }
        }
    }
}
