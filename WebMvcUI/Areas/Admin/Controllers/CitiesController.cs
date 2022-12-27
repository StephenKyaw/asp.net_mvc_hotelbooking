using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebMvcUI.Areas.Admin.Controllers
{
    [Authorize]
    [Area("admin")]
    public class CitiesController : Controller
    {

        private readonly ILocationService _locationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CitiesController(ILocationService locationService, UserManager<ApplicationUser> userManager)
        {
            _locationService = locationService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var cities = await _locationService.GetCities();

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "City Listing";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "dashboard", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Dashboard" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "City Listing", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;


            return View(
                cities.Select(x => new CityViewModel
                {
                    Id = x.CityId,
                    CityName = x.CityName,
                    CreatedDate = x.CreatedDate,
                    CreatedBy = x.CreatedBy,
                    LastModifiedDate = x.LastModifiedDate,
                    LastModifiedBy = x.LastModifiedBy
                })
                );
        }

        public IActionResult Create()
        {
            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Create City";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "cities", ActionName = "index", AreaName = "admin", IsActive = false, Title = "City Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Create City", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CityViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                City city = new City()
                {
                    CityName = model.CityName,
                    CreatedBy = user != null ? user.UserName : string.Empty,
                };

                await _locationService.AddCity(city);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(string id)
        {
            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Update City";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "cities", ActionName = "index", AreaName = "admin", IsActive = false, Title = "City Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Update City", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            if (id == null)
            {
                return NotFound();
            }

            City city = await _locationService.GetCityById(id);

            if (city == null)
            {
                return NotFound();
            }
            return View(new CityViewModel { Id = city.CityId, CityName = city.CityName });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, CityViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updateCity = await _locationService.GetCityById(id);

                if (updateCity != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    updateCity.CityName = model.CityName;
                    updateCity.LastModifiedBy = user != null ? user.UserName : string.Empty;
                    updateCity.LastModifiedDate = DateTime.Now;

                    await _locationService.UpdateCity(updateCity);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Delete City";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "cities", ActionName = "index", AreaName = "admin", IsActive = false, Title = "City Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Delete City", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            if (id == null)
            {
                return NotFound();
            }
            var city = await _locationService.GetCityById(id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await _locationService.DeleteCity(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
