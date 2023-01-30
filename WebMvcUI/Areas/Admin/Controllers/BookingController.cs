using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebMvcUI.Areas.Admin.Controllers
{
    [Authorize(Roles = Constants.ROLE_ADMINISTRATORS)]
    [Area("admin")]
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;
        private readonly IRoomService _roomService;
        private readonly IHotelService _hotelService;

        public BookingsController(IBookingService bookingService, IRoomService roomService, IHotelService hotelService)
        {
            _bookingService = bookingService;
            _roomService = roomService;
            _hotelService = hotelService;
        }

        public async Task<IActionResult> Index()
        {
            var _bookingList = await _bookingService.GetBookingList();

            var _bookingViewModel = new List<BookingsViewModel>();

            if (_bookingList != null)
            {
                foreach (var booking in _bookingList)
                {
                    var bookingViewModel = new BookingsViewModel();
                    bookingViewModel.BookingDate = booking.BookingDate;
                    bookingViewModel.Remark = booking.Remark;
                    bookingViewModel.CustomerID = booking.CustomerID;
                    bookingViewModel.CustomerName = booking.CustomerName;
                    bookingViewModel.TotalAmount = booking.TotalAmount;

                    var items = booking.BookingItems.ToList();

                    foreach (var item in items)
                    {
                        var _bookingItemViewModel = new BookingsItemViewModel();
                        _bookingItemViewModel.RoomId = item.RoomId;
                        _bookingItemViewModel.Description = item.Description;
                        _bookingItemViewModel.price = item.Price;
                        _bookingItemViewModel.Amount = item.Amount;
                        _bookingItemViewModel.NumberOfRooms = item.NumberOfRooms;

                        var _room = await _roomService.GetRoomById(item.RoomId);

                        var _hotel = await _hotelService.GetHotelById(_room.HotelId);

                        _bookingItemViewModel.RoomType = _room.RoomType.RoomTypeName;
                        _bookingItemViewModel.City = _hotel.City.CityName;
                        _bookingItemViewModel.Township = _hotel.Township.TownshipName;
                        _bookingItemViewModel.HotelName = _hotel.Name;


                        bookingViewModel.Items.Add(_bookingItemViewModel);

                    }

                    _bookingViewModel.Add(bookingViewModel);
                }
            }

            return View(_bookingViewModel);
        }
    }
}
