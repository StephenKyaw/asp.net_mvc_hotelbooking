using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace WebMvcUI.Areas.Admin.Controllers
{
    [Authorize(Roles = Constants.ROLE_ADMINISTRATORS)]
    [Area("admin")]
    public class TownshipsController : Controller
    {
        private readonly ILocationService _locationService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TownshipsController(ILocationService locationService, UserManager<ApplicationUser> userManager)
        {
            _locationService = locationService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Township Listing";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "dashboard", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Dashboard" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Township Listing", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            var townships = await _locationService.GetTownships();

            return View(townships.Select(x => new TownshipViewModel
            {
                Id = x.TownshipId,
                TownshipName = x.TownshipName,
                CityId = x.CityId,
                CityName = x.City.CityName,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                LastModifiedBy = x.LastModifiedBy,
                LastModifiedDate = x.LastModifiedDate
            }));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Township Create";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "townships", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Township Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Township Create", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            ViewData["CityId"] = new SelectList(await _locationService.GetCities(), "CityId", "CityName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TownshipViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var townshop = new Township()
                {
                    TownshipName = model.TownshipName,
                    CityId = model.CityId,
                    CreatedBy = user != null ? user.UserName : string.Empty
                };

                await _locationService.AddTownship(townshop);

                return RedirectToAction(nameof(Index));
            }

            ViewData["CityId"] = new SelectList(await _locationService.GetCities(), "CityId", "CityName", model.CityId);

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Township Update";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "townships", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Township Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Township Update", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            if (id == null)
            {
                return NotFound();
            }

            var township = await _locationService.GetTownshipById(id);

            if (township == null)
            {
                return NotFound();
            }

            ViewData["CityId"] = new SelectList(await _locationService.GetCities(), "CityId", "CityName", township.CityId);

            return View(new TownshipViewModel { Id = township.TownshipId, TownshipName = township.TownshipName, CityId = township.CityId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, TownshipViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updateTownShip = await _locationService.GetTownshipById(model.Id);

                if (updateTownShip != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    updateTownShip.TownshipName = model.TownshipName;
                    updateTownShip.CityId = model.CityId;
                    updateTownShip.LastModifiedBy = user != null ? user.UserName : string.Empty;
                    updateTownShip.LastModifiedDate = DateTime.Now;

                    await _locationService.UpdateTownship(updateTownShip);
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CityId"] = new SelectList(await _locationService.GetCities(), "CityId", "CityName", model.CityId);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Township Delete";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "townships", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Township Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Township Delete", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            if (id == null)
            {
                return NotFound();
            }

            var township = await _locationService.GetTownshipById(id);

            if (township == null)
            {
                return NotFound();
            }

            return View(new TownshipViewModel
            {
                Id = township.TownshipId,
                TownshipName = township.TownshipName,
                CityName = township.City.CityName,
                CityId = township.CityId,
                CreatedDate = township.CreatedDate,
                CreatedBy = township.CreatedBy,
                LastModifiedBy = township.LastModifiedBy,
                LastModifiedDate = township.LastModifiedDate
            });
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var township = await _locationService.GetTownshipById(id);

            if (township != null)
            {
                await _locationService.DeleteTownship(township.TownshipId);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
