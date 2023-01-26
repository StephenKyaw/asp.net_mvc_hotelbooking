using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Text.Json;

namespace WebMvcUI.Areas.Admin.Controllers
{
    [Authorize(Roles = Constants.ROLE_ADMINISTRATORS)]
    [Area("admin")]
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IHotelService _hotelService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoomsController(IRoomService roomService, IHotelService hotelService, UserManager<ApplicationUser> userManager)
        {
            _roomService = roomService;
            _hotelService = hotelService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Room Listing";

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Room Listing";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "dashboard", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Dashboard" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Room Listing", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            var rooms = await _roomService.GetRooms();

            return View(GetViewModels(rooms));
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Title = "Room Create";

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Room Create";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "rooms", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Room Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Room Create", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            RoomViewModel model = new RoomViewModel();

            ViewData["HotelId"] = new SelectList(await _hotelService.GetHotels(), "HotelId", "Name");
            ViewData["RoomTypeId"] = new SelectList(await _roomService.GetRoomTypes(), "RoomTypeId", "RoomTypeName");

            List<JsonDataItem> betTypes = (await _roomService.GetBedTypes())
                            .Select(x => new JsonDataItem { text = x.BedTypeName, value = x.BedTypeId })
                            .ToList();

            betTypes.Insert(0, new JsonDataItem { text = "Select One", value = string.Empty });

            ViewData["BedTypes"] = JsonSerializer.Serialize(betTypes);

            model.RoomFacilities = (await _roomService.GetFacilityTypes())
                            .Select(x => new SelectListItem() { Text = x.FacilityTypeName, Value = x.FacilityTypeId }).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                Room room = new Room();
                room.HotelId = model.HotelId;
                room.RoomTypeId = model.RoomTypeId;
                room.Price = model.Price;
                room.NumberOfRooms = model.NumberOfRooms;
                room.Rate = model.Rate;

                room.CreatedBy = user != null ? user.UserName : string.Empty;

                if (!string.IsNullOrEmpty(model.RoomBedsJsonString))
                {
                    model.RoomBeds = JsonSerializer.Deserialize<List<RoomBedViewModel>>(model.RoomBedsJsonString);

                    if (model.RoomBeds != null && model.RoomBeds.Count() > 0)
                    {
                        List<RoomBed> roomBeds = new List<RoomBed>();

                        foreach (var item in model.RoomBeds)
                        {
                            var roomBed = new RoomBed
                            {
                                RoomId = room.RoomId,
                                BedTypeId = item.BedTypes.FirstOrDefault().value,
                                NumberOfBeds = Convert.ToInt32(item.NumberOfBeds)
                            };

                            roomBeds.Add(roomBed);
                        }

                        room.RoomBeds = roomBeds;
                    }
                }

                if (model.FileRoomPhotos.Count > 0)
                {
                    room.RoomPhotos = await GetRoomPhotos(model.FileRoomPhotos, room.RoomId);
                }

                var _roomFacilitiesId = model.RoomFacilities.Where(x => x.Selected).Select(x => x.Value).ToList();

                if (_roomFacilitiesId.Count > 0)
                {
                    List<RoomFacility> _facilityList = new List<RoomFacility>();

                    foreach (var id in _roomFacilitiesId)
                    {
                        RoomFacility roomFacility = new RoomFacility()
                        {
                            RoomId = room.RoomId,
                            FacilityTypeId = id
                        };

                        _facilityList.Add(roomFacility);
                    }

                    room.RoomFacilities = _facilityList;
                }

                await _roomService.AddRoom(room);

                return RedirectToAction(nameof(Index));
            }

            ViewData["HotelId"] = new SelectList(await _hotelService.GetHotels(), "HotelId", "Name", model.HotelId);
            ViewData["RoomTypeId"] = new SelectList(await _roomService.GetRoomTypes(), "RoomTypeId", "RoomTypeName", model.RoomTypeId);

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Title = "Room Update";

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Room Update";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "rooms", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Room Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Room Update", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            if (id == null)
            {
                return NotFound();
            }

            var room = await _roomService.GetRoomById(id);

            if (room == null)
            {
                return NotFound();
            }
            RoomViewModel model = BindViewModel(room);

            ViewData["HotelId"] = new SelectList(await _hotelService.GetHotels(), "HotelId", "Name", room.HotelId);
            ViewData["RoomTypeId"] = new SelectList(await _roomService.GetRoomTypes(), "RoomTypeId", "RoomTypeName", room.RoomTypeId);

            var betTypes = (await _roomService.GetBedTypes())
                         .Select(x => new { text = x.BedTypeName, value = x.BedTypeId })
                         .ToList();

            ViewData["BedTypes"] = JsonSerializer.Serialize(betTypes);


            model.RoomFacilities = (await _roomService.GetFacilityTypes())
                            .Select(x => new SelectListItem() { Text = x.FacilityTypeName, Value = x.FacilityTypeId }).ToList();

            if (room.RoomFacilities.Any())
            {
                foreach (var item in model.RoomFacilities)
                {
                    var _check = room.RoomFacilities.Any(x => x.FacilityTypeId == item.Value);

                    if (_check)
                    {
                        item.Selected = true;
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, RoomViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                Room room = await _roomService.GetRoomById(model.Id);
                room.HotelId = model.HotelId;
                room.RoomTypeId = model.RoomTypeId;
                room.Price = model.Price;
                room.NumberOfRooms = model.NumberOfRooms;
                room.Rate = model.Rate;
                room.LastModifiedBy = user != null ? user.UserName : string.Empty;
                room.LastModifiedDate = DateTime.Now;

                if (!string.IsNullOrEmpty(model.RoomBedsJsonString))
                {
                    model.RoomBeds = JsonSerializer.Deserialize<List<RoomBedViewModel>>(model.RoomBedsJsonString);

                    if (model.RoomBeds != null && model.RoomBeds.Count() > 0)
                    {
                        List<RoomBed> roomBeds = new List<RoomBed>();

                        foreach (var item in model.RoomBeds)
                        {
                            var roomBed = new RoomBed
                            {
                                RoomId = room.RoomId,
                                BedTypeId = item.BedTypes != null ? item.BedTypes.FirstOrDefault().value : string.Empty,
                                NumberOfBeds = Convert.ToInt32(item.NumberOfBeds)
                            };

                            roomBeds.Add(roomBed);
                        }

                        room.RoomBeds = roomBeds;
                    }
                }

                if (model.FileRoomPhotos.Count > 0)
                {
                    room.RoomPhotos = await GetRoomPhotos(model.FileRoomPhotos, room.RoomId);
                }

                var _roomFacilitiesId = model.RoomFacilities.Where(x => x.Selected).Select(x => x.Value).ToList();

                if (_roomFacilitiesId.Count > 0)
                {
                    List<RoomFacility> _facilityList = new List<RoomFacility>();

                    foreach (var _id in _roomFacilitiesId)
                    {
                        RoomFacility roomFacility = new RoomFacility()
                        {
                            RoomId = room.RoomId,
                            FacilityTypeId = _id
                        };

                        _facilityList.Add(roomFacility);
                    }

                    room.RoomFacilities = _facilityList;
                }

                await _roomService.UpdateRoom(room);

                return RedirectToAction(nameof(Index));
            }


            ViewData["HotelId"] = new SelectList(await _hotelService.GetHotels(), "HotelId", "Name", model.HotelId);
            ViewData["RoomTypeId"] = new SelectList(await _roomService.GetRoomTypes(), "RoomTypeId", "RoomTypeName", model.RoomTypeId);

            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            ViewBag.Title = "Room Delete";

            ContentHeaderViewModel contentHeader = new ContentHeaderViewModel();
            contentHeader.Title = "Room Delete";
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { ControllerName = "rooms", ActionName = "index", AreaName = "admin", IsActive = false, Title = "Room Listing" });
            contentHeader.Breadcrumbs.Add(new BreadcrumbViewModel { Title = "Room Delete", IsActive = true });

            ViewData["ContentHeader"] = contentHeader;

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var room = await _roomService.GetRoomById(id);

            if (room == null)
            {
                return NotFound();
            }

            return View(BindViewModel(room));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            await _roomService.DeleteRoom(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<RoomPhoto>> GetRoomPhotos(IFormFileCollection files, string roomId)
        {
            List<RoomPhoto> photos = new List<RoomPhoto>();

            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    RoomPhoto photo = new RoomPhoto();

                    string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Rooms");

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

                    photo.RoomId = roomId;
                    photo.FileName = _fileName;
                    photo.OriginalFileName = file.FileName;
                    photo.ContentType = file.ContentType;

                    photos.Add(photo);
                }

            }
            return photos;
        }

        private IEnumerable<RoomViewModel> GetViewModels(IEnumerable<Room> rooms)
        {
            return rooms.Select(x => BindViewModel(x));
        }

        private RoomViewModel BindViewModel(Room room)
        {
            RoomViewModel model = new RoomViewModel();
            model.Id = room.RoomId;
            model.HotelId = room.HotelId;
            model.HotelName = room.Hotel.Name;
            model.RoomTypeId = room.RoomTypeId;
            model.RoomTypeName = room.RoomType.RoomTypeName;
            model.Price = room.Price;
            model.Rate = room.Rate;
            model.NumberOfRooms = room.NumberOfRooms;

            if (room.RoomBeds != null)
            {
                List<RoomBedViewModel> roomBedViewModels = new List<RoomBedViewModel>();
                foreach (var roomBed in room.RoomBeds)
                {
                    RoomBedViewModel roomBedViewModel = new RoomBedViewModel();
                    string _bedTypeName = _roomService.GetBedTypeById(roomBed.BedTypeId).BedTypeName;
                    roomBedViewModel.BedTypes = new List<JsonDataItem>() { new JsonDataItem { text = _bedTypeName, value = roomBed.BedTypeId } };
                    roomBedViewModel.NumberOfBeds = roomBed.NumberOfBeds.ToString();
                    roomBedViewModels.Add(roomBedViewModel);
                }
                model.RoomBeds = roomBedViewModels;
                model.RoomBedsJsonString = JsonSerializer.Serialize(model.RoomBeds.ToList());
            }

            model.RoomPhotos = room.RoomPhotos.Select(x => new RoomPhotoViewModel
            {
                RoomId = x.RoomId,
                ContentType = x.ContentType,
                FileName = x.FileName,
                OriginalFileName = x.OriginalFileName,
            }).ToList();

            return model;
        }
    }
}
