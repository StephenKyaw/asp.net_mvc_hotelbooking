using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebMvcUI.Areas.Admin.Controllers
{
    [Area("admin")]
    public class RoomTypesController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoomTypesController(IRoomService roomService, UserManager<ApplicationUser> userManager)
        {
            _roomService = roomService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "RoomType Listing";
            
            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "RoomType Listing";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "dashboard", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Dashboard" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "RoomType Listing", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            var roomTypes = await _roomService.GetRoomTypes();

            return View(GetViewModels(roomTypes));
        }

        public IActionResult Create()
        {
            ViewBag.Title = "RoomType Create";
            
            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "RoomType Listing";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "roomtypes", ActionName = "index", AreaName = "admin", IsActive = false, Title = "RoomType Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "RoomType Create", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var roomType = new RoomType { RoomTypeName = model.RoomTypeName, CreatedBy = user != null ? user.UserName : string.Empty };
                await _roomService.AddRoomType(roomType);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Title = "RoomType Update";

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "RoomType Listing";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "roomtypes", ActionName = "index", AreaName = "admin", IsActive = false, Title = "RoomType Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "RoomType Update", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            if (id == null)
            {
                return NotFound();
            }

            var roomType = await _roomService.GetRoomTypeById(id);

            if (roomType == null)
            {
                return NotFound();
            }
            return View(BindViewModel(roomType));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, RoomTypeViewModel model)
        {
            if (id != model.RoomTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updateRoomType = await _roomService.GetRoomTypeById(model.RoomTypeId);

                if (updateRoomType != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    updateRoomType.LastModifiedBy = user != null ? user.UserName : string.Empty;
                    updateRoomType.LastModifiedDate = DateTime.Now;
                    updateRoomType.RoomTypeName = model.RoomTypeName;

                    await _roomService.UpdateRoomType(updateRoomType);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            ViewBag.Title = "RoomType Delete";

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "RoomType Listing";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "roomtypes", ActionName = "index", AreaName = "admin", IsActive = false, Title = "RoomType Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "RoomType Delete", IsActive = true });

            if (id == null)
            {
                return NotFound();
            }

            var roomType = await _roomService.GetRoomTypeById(id);

            if (roomType == null)
            {
                return NotFound();
            }
            return View(BindViewModel(roomType));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            await _roomService.DeleteRoomType(id);

            return RedirectToAction(nameof(Index));
        }

        private IEnumerable<RoomTypeViewModel> GetViewModels(IEnumerable<RoomType> roomTypes)
        {
            return roomTypes.Select(x => BindViewModel(x));
        }

        private RoomTypeViewModel BindViewModel(RoomType roomType)
        {
            return new RoomTypeViewModel()
            {
                RoomTypeId = roomType.RoomTypeId,
                RoomTypeName = roomType.RoomTypeName,
                CreatedBy = roomType.CreatedBy,
                CreatedDate = roomType.CreatedDate,
                LastModifiedBy = roomType.LastModifiedBy,
                LastModifiedDate = roomType.LastModifiedDate,
            };
        }
    }
}
