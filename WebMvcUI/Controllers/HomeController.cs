using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using WebMvcUI.Models;

namespace WebMvcUI.Controllers
{

    public class HomeController : Controller
    {
        private readonly IRoomService _roomService;

        public HomeController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _roomService.GetRooms();

            return View(rooms);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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