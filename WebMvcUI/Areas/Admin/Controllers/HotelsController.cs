using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace WebMvcUI.Areas.Admin.Controllers
{
    [Authorize(Roles = Constants.ROLE_ADMINISTRATORS)]
    [Area("admin")]
    public class HotelsController : Controller
    {
       
        private readonly ILocationService _locationService;
        private readonly IHotelService _hotelService;
        private readonly UserManager<ApplicationUser> _userManager;
        public HotelsController(ILocationService locationService, IHotelService hotelService, UserManager<ApplicationUser> userManager)
        {
            _locationService = locationService;
            _hotelService = hotelService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Hotel Listing";

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Hotel Listing";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "dashboard", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Dashboard" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Hotel Listing", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            var hotels = await _hotelService.GetHotels();

            IEnumerable<HotelViewModel> model = GetHotelViewModelList(hotels);

            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Title = "Hotel Create";

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Hotel Create";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "hotels", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Hotel Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Hotel Create", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            ViewData["CityId"] = new SelectList(await _locationService.GetCities(), "CityId", "CityName");
            ViewData["TownshipId"] = new SelectList(await _locationService.GetTownships(), "TownshipId", "TownshipName");
          
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotelViewModel model, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                var hotel = new Hotel();
                hotel.Name = model.Name;
                hotel.Description = model.Description;
                hotel.Address = model.Address;
                hotel.Email = model.Email;
                hotel.Phone_1 = model.Phone_1;
                hotel.Phone_2 = model.Phone_2;
                hotel.Phone_3 = model.Phone_3;
                hotel.CityId = model.CityId;
                hotel.TownshipId = model.TownshipId;

                if (model.FileRoomPhotos.Count > 0)
                {
                    hotel.HotelPhotos = await GetHotelPhotos(model.FileRoomPhotos, hotel.HotelId);
                }

                await _hotelService.AddHotel(hotel);

                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(await _locationService.GetCities(), "CityId", "CityName", model.CityId);
            ViewData["TownshipId"] = new SelectList(await _locationService.GetTownships(), "TownshipId", "TownshipName", model.TownshipId);

            return View(model);
        }
        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Title = "Hotel Update";

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Hotel Update";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "hotels", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Hotel Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Hotel Update", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _hotelService.GetHotelById(id);

            if (hotel == null)
            {
                return NotFound();
            }

            ViewData["CityId"] = new SelectList(await _locationService.GetCities(), "CityId", "CityName", hotel.CityId);
            ViewData["TownshipId"] = new SelectList(await _locationService.GetTownships(), "TownshipId", "TownshipName", hotel.TownshipId);

            HotelViewModel model = BindHotelViewModel(hotel);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, HotelViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var hotelUpdate = await _hotelService.GetHotelById(model.Id);

                if (hotelUpdate != null)
                {
                    var user = await _userManager.GetUserAsync(User);

                    hotelUpdate.LastModifiedBy = user != null ? user.UserName : string.Empty;
                    hotelUpdate.LastModifiedDate = DateTime.Now;
                    hotelUpdate.Name = model.Name;
                    hotelUpdate.Description = model.Description;
                    hotelUpdate.Address = model.Address;
                    hotelUpdate.Email = model.Email;
                    hotelUpdate.Phone_1 = model.Phone_1;
                    hotelUpdate.Phone_2 = model.Phone_2;
                    hotelUpdate.Phone_3 = model.Phone_3;
                    hotelUpdate.CityId = model.CityId;
                    hotelUpdate.TownshipId = model.TownshipId;

                    if (model.FileRoomPhotos.Count > 0)
                    {
                        if (hotelUpdate.HotelPhotos.Count() > 0)
                        {
                            RemoveImages(hotelUpdate.HotelPhotos.ToList());
                            await _hotelService.DeleteHotelPhotosByHotelId(hotelUpdate.HotelId);
                        }

                        hotelUpdate.HotelPhotos = await GetHotelPhotos(model.FileRoomPhotos, hotelUpdate.HotelId);
                    }

                    await _hotelService.UpdateHotel(hotelUpdate);
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CityId"] = new SelectList(await _locationService.GetCities(), "CityId", "CityName", model.CityId);
            ViewData["TownshipId"] = new SelectList(await _locationService.GetTownships(), "TownshipId", "TownshipName", model.TownshipId);

            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            ViewBag.Title = "Hotel Delete";

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Hotel Delete";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "hotels", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Hotel Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Hotel Delete", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _hotelService.GetHotelById(id);

            if (hotel == null)
            {
                return NotFound();
            }

            return View(BindHotelViewModel(hotel));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            await _hotelService.DeleteHotel(id);

            return RedirectToAction(nameof(Index));
        }

        private void RemoveImages(List<HotelPhoto> photos)
        {
            foreach (var photo in photos)
            {
                string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Hotels");

                string _fullPath = Path.Combine(_folderPath, photo.FileName);

                if (System.IO.File.Exists(_fullPath))
                {
                    System.IO.File.Delete(_fullPath);
                }
            }
        }

        private IEnumerable<HotelViewModel> GetHotelViewModelList(IEnumerable<Hotel> hotels)
        {
            return hotels.Select(x => BindHotelViewModel(x));
        }

        private HotelViewModel BindHotelViewModel(Hotel hotel)
        {
            return new HotelViewModel()
            {
                Id = hotel.HotelId,
                Name = hotel.Name,
                Description = hotel.Description,
                Email = hotel.Email,
                Address = hotel.Address,
                Phone_1 = hotel.Phone_1,
                Phone_2 = hotel.Phone_2,
                Phone_3 = hotel.Phone_3,
                CityId = hotel.CityId,
                CityName = hotel.City.CityName,
                TownshipId = hotel.TownshipId,
                TownshipName = hotel.Township.TownshipName,
                CreatedBy = hotel.CreatedBy,
                CreatedDate = hotel.CreatedDate,
                HotelPhotos = hotel.HotelPhotos.Select(x => new HotelPhotoViewModel { FileName = x.FileName, ContentType = x.ContentType, HotelId = x.HotelId, OriginalFileName = x.OriginalFileName }).ToList()

            };
        }
        private async Task<List<HotelPhoto>> GetHotelPhotos(IFormFileCollection files, string hotelId)
        {
            List<HotelPhoto> photos = new List<HotelPhoto>();

            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    HotelPhoto photo = new HotelPhoto();

                    string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Hotels");

                    if (!Directory.Exists(_folderPath))
                    {
                        Directory.CreateDirectory(_folderPath);
                    }

                    string _fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string _fileNameWithPath = Path.Combine(_folderPath, _fileName);

                    using (var straem = new FileStream(_fileNameWithPath, FileMode.Create))
                    {
                        await file.CopyToAsync(straem);
                    }

                    photo.HotelId = hotelId;
                    photo.FileName = _fileName;
                    photo.OriginalFileName = file.FileName;
                    photo.ContentType = file.ContentType;

                    photos.Add(photo);
                }

            }
            return photos;
        }
    }
}
