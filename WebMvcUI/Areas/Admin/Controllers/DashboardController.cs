using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebMvcUI.Areas.Admin.Controllers
{    
    [Area("admin")]
    [Authorize(Roles = Constants.ROLE_ADMINISTRATORS)]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            SetContentHeader();

            return View();
        }

        private void SetContentHeader()
        {
            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Dashboard";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "dashboard", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Home" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Dashbaord", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;
        }
    }
}
