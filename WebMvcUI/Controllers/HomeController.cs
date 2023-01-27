using Domain.Entities.Bookings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Xml.Linq;
using WebMvcUI.Models;
using WebMvcUI.Extensions;

namespace WebMvcUI.Controllers
{
   

    public class HomeController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBookingService _bookingService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(IRoomService roomService, UserManager<ApplicationUser> userManager, IBookingService bookingService, SignInManager<ApplicationUser> signInManager)
        {
            _roomService = roomService;
            _userManager = userManager;
            _bookingService = bookingService;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _roomService.GetRooms();

            return View(GetViewModels(rooms));
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var _loginUser = await _userManager.FindByEmailAsync(model.Email);

                if (_loginUser != null)
                {
                    var _checkPassword = await _userManager.CheckPasswordAsync(_loginUser, model.Password);

                    if (_checkPassword)
                    {
                        await _signInManager.SignInAsync(_loginUser, model.RememberMe);

                        var _isAdminRole = await _userManager.IsInRoleAsync(_loginUser, Constants.ROLE_ADMINISTRATORS);

                        if (_isAdminRole)
                        {
                            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }

            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CustomerRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                user.Email = model.Email;
                user.UserName = model.Email;
                user.FullName = model.FullName;

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Constants.ROLE_USER);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }

            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Remove("Booking");

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Reserve()
        {
            var user = await _userManager.GetUserAsync(User);
            BookingViewModel model = new BookingViewModel();

            List<BookingItemViewModel> bookingList = GetBookingList();


            model.BookingItems = bookingList;
            model.TotalAmount = bookingList.Sum(x => x.Amount);
            model.CustomerName = user.UserName;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reserve(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            BookingViewModel model = new BookingViewModel();

            List<BookingItemViewModel> bookingList = GetBookingList();

            if (!string.IsNullOrEmpty(id))
            {
                var room = await _roomService.GetRoomById(id);

                BookingItemViewModel booking = new BookingItemViewModel();
                booking.Id = Guid.NewGuid().ToString();
                booking.RoomId = id;
                booking.Description = $"{room.Hotel.Name}, {room.RoomType.RoomTypeName}";
                booking.Price = room.Price;
                booking.NumberOfRooms = 1;
                booking.Amount = room.Price * 1;

                bookingList.Add(booking);
            }


            HttpContext.Session.Set<List<BookingItemViewModel>>("Booking", bookingList);

            model.BookingItems = bookingList;
            model.TotalAmount = bookingList.Sum(x => x.Amount);
            model.CustomerName = user.UserName;

            return RedirectToAction("Index", "Home");
        }
        public ActionResult RemoveBookingItem(string id)
        {
            List<BookingItemViewModel> bookingList = GetBookingList();

            var removeItem = bookingList.FirstOrDefault(x => x.Id == id);

            bookingList.Remove(removeItem);

            HttpContext.Session.Set<List<BookingItemViewModel>>("Booking", bookingList);

            return RedirectToAction("Reserve");
        }
        [HttpPost]
        public async Task<IActionResult> BookingConfirm(BookingViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            List<BookingItemViewModel> bookingList = GetBookingList();

            Booking booking = new Booking();
            booking.BookingDate = model.BookingDate;
            booking.Remark = model.Remark;
            booking.TotalAmount = bookingList.Sum(x => x.Price);
            booking.CustomerID = user.Id.ToString();
            booking.CustomerName = user.UserName;
            booking.BookingID = Guid.NewGuid().ToString();

            List<BookingItem> bookingItems = new List<BookingItem>();

            foreach (var item in bookingList)
            {
                BookingItem bookingItem = new BookingItem();
                bookingItem.BookingItemID = Guid.NewGuid().ToString();
                bookingItem.BookingID = booking.BookingID;
                bookingItem.RoomId = item.RoomId;
                bookingItem.Description = item.Description;
                bookingItem.NumberOfRooms = item.NumberOfRooms;
                bookingItem.Price = item.Price;
                bookingItem.Amount = item.Amount;

                bookingItems.Add(bookingItem);
            }

            booking.BookingItems = bookingItems;

            await _bookingService.AddBooking(booking);


            return RedirectToAction("BookingConfirmComplete", new { name = booking.CustomerName, date = booking.BookingDate });
        }

        public IActionResult BookingConfirmComplete(string name, DateTime date)
        {
            HttpContext.Session.Remove("Booking");

            ViewBag.Message = $"Thank you for booking an appointment,  " +
                $"with {name} on {date}. " +
                $"Please text CONFIRM to confirm your appointment, " +
                $"CANCEL to cancel it or call us at +9512345678 if you wish to reschedule. " +
                $"We look forward to seeing you! ";
            return View();
        }
        private List<BookingItemViewModel> GetBookingList()
        {
            List<BookingItemViewModel> bookingList;

            var sessionValues = HttpContext.Session.Get<List<BookingItemViewModel>>("Booking");

            if (sessionValues != null)
            {
                bookingList = sessionValues.ToList();
            }
            else
            {
                bookingList = new List<BookingItemViewModel>();
            }

            return bookingList;
        }
        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'");
            }
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
                model.RoomBedsJsonString = System.Text.Json.JsonSerializer.Serialize(model.RoomBeds.ToList());
            }

            model.RoomPhotos = room.RoomPhotos.Select(x => new RoomPhotoViewModel
            {
                RoomId = x.RoomId,
                ContentType = x.ContentType,
                FileName = x.FileName,
                OriginalFileName = x.OriginalFileName,
            }).ToList();

            if (room.RoomFacilities.Any())
            {
                foreach (var facility in room.RoomFacilities)
                {
                    var facilityType = _roomService.GetFacilityTypeById(facility.FacilityTypeId);

                    model.RoomFacilityList.Add(facilityType.FacilityTypeName);
                }
            }
            return model;
        }
    }
}