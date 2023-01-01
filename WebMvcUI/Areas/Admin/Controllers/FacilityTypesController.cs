using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebMvcUI.Areas.Admin.Controllers
{
    [Authorize(Roles = Constants.ROLE_ADMINISTRATORS)]
    [Area("admin")]
    public class FacilityTypesController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly UserManager<ApplicationUser> _userManager;

        public FacilityTypesController(IRoomService roomService, UserManager<ApplicationUser> userManager)
        {
            _roomService = roomService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Facility Type Listing";

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Facility Type Listing";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "dashboard", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Dashboard" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Facility Type Listing", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            var _facilities = await _roomService.GetFacilityTypes();

            return View(GetViewModels(_facilities));
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Facility Type Create";

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Facility Type Create";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "facilitytypes", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Facility Type Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Facility Type Create", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FacilityTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var facilityType = new FacilityType
                {
                    FacilityTypeName = model.FacilityTypeName,
                    CreatedBy = user != null ? user.UserName : string.Empty
                };
                await _roomService.AddFacilityType(facilityType);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Title = "Facility Type Update";

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Facility Type Update";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "facilitytypes", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Facility Type Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Facility Type Update", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            if (id == null)
            {
                return NotFound();
            }

            var facilityType = await _roomService.GetFacilityTypeById(id);

            if (facilityType == null)
            {
                return NotFound();
            }

            return View(BindViewModel(facilityType));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, FacilityTypeViewModel model)
        {
            if (id != model.FacilityTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var facilityTypeUpdate = await _roomService.GetFacilityTypeById(model.FacilityTypeId);


                if (facilityTypeUpdate != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    facilityTypeUpdate.LastModifiedBy = user != null ? user.UserName : string.Empty;
                    facilityTypeUpdate.LastModifiedDate = DateTime.Now;
                    facilityTypeUpdate.FacilityTypeName = model.FacilityTypeName;

                    await _roomService.UpdateFacilityType(facilityTypeUpdate);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            ViewBag.Title = "Facility Type Delete";

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Facility Type Delete";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "facilitytypes", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Facility Type Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Facility Type Delete", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            if (id == null)
            {
                return NotFound();
            }

            var facilityType = await _roomService.GetFacilityTypeById(id);

            if (facilityType == null)
            {
                return NotFound();
            }

            return View(BindViewModel(facilityType));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            await _roomService.DeleteFacilityType(id);

            return RedirectToAction(nameof(Index));
        }

        private IEnumerable<FacilityTypeViewModel> GetViewModels(IEnumerable<FacilityType> facilityTypes)
        {
            return facilityTypes.Select(x => BindViewModel(x));
        }

        private FacilityTypeViewModel BindViewModel(FacilityType facilityType)
        {
            return new FacilityTypeViewModel()
            {
                FacilityTypeId = facilityType.FacilityTypeId,
                FacilityTypeName = facilityType.FacilityTypeName,
                CreatedBy = facilityType.CreatedBy,
                CreatedDate = facilityType.CreatedDate,
                LastModifiedBy = facilityType.LastModifiedBy,
                LastModifiedDate = facilityType.LastModifiedDate,
            };
        }
    }
}
