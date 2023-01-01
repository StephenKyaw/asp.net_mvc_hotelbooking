using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebMvcUI.Areas.Admin.Controllers
{
    [Authorize(Roles = Constants.ROLE_ADMINISTRATORS)]
    [Area("admin")]
    public class BedTypesController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BedTypesController(IRoomService roomService, UserManager<ApplicationUser> userManager)
        {
            _roomService = roomService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "BedType Listing";

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "BedType Listing";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "dashboard", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Dashboard" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "BedType Listing", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            var bedtypes = await _roomService.GetBedTypes();

            return View(GetViewModels(bedtypes));
        }

        public IActionResult Create()
        {
            ViewBag.Title = "BedType Create";

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Create BedType";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "bedtypes", ActionName = "index", AreaName = "admin", IsActive = false, Title = "BedType Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Create BedType", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BedTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var bedType = new BedType()
                {
                    BedTypeName = model.BedTypeName,
                    CreatedBy = user != null ? user.UserName : string.Empty
                };

                await _roomService.AddBedType(bedType);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Title = "BedType Update";

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Update BedType";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "bedtypes", ActionName = "index", AreaName = "admin", IsActive = false, Title = "BedType Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Update BedType", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            if (id == null)
            {
                return NotFound();
            }

            var bedType = await _roomService.GetBedTypeByIdAsync(id);

            if (bedType == null)
            {
                return NotFound();
            }

            return View(BindViewModel(bedType));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, BedTypeViewModel model)
        {
            if (id != model.BedTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var bedTypeUpdate = await _roomService.GetBedTypeByIdAsync(model.BedTypeId);

                if (bedTypeUpdate != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    bedTypeUpdate.LastModifiedBy = user != null ? user.UserName : string.Empty;
                    bedTypeUpdate.LastModifiedDate = DateTime.Now;
                    bedTypeUpdate.BedTypeName = model.BedTypeName;

                    await _roomService.UpdateBedType(bedTypeUpdate);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            ViewBag.Title = "BedType Delete";
            
            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Delete BedType";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "bedtypes", ActionName = "index", AreaName = "admin", IsActive = false, Title = "BedType Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Delete BedType", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            if (id == null)
            {
                return NotFound();
            }


            var bedType = await _roomService.GetBedTypeByIdAsync(id);

            if (bedType == null)
            {
                return NotFound();
            }

            return View(BindViewModel(bedType));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            await _roomService.DeleteBedType(id);

            return RedirectToAction(nameof(Index));
        }

        private IEnumerable<BedTypeViewModel> GetViewModels(IEnumerable<BedType> bedTypes)
        {
            return bedTypes.Select(x => BindViewModel(x));
        }

        private BedTypeViewModel BindViewModel(BedType bedType)
        {
            return new BedTypeViewModel()
            {
                BedTypeId = bedType.BedTypeId,
                BedTypeName = bedType.BedTypeName,
                CreatedBy = bedType.CreatedBy,
                CreatedDate = bedType.CreatedDate,
                LastModifiedBy = bedType.LastModifiedBy,
                LastModifiedDate = bedType.LastModifiedDate,
            };
        }
    }
}
