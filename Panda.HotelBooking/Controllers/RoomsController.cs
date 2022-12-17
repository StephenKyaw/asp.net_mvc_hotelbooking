using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Panda.HotelBooking.Data;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;
using NuGet.Packaging;
using Panda.HotelBooking.Models;

namespace Panda.HotelBooking.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public RoomsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Room Listing";

            var data = await _context.Rooms
                .Include(r => r.CreatedUser)
                .Include(r => r.Hotel)
                .Include(r => r.RoomType)
                .Include(r => r.UpdatedUser).ToListAsync();
            return View(data);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.CreatedUser)
                .Include(r => r.Hotel)
                .Include(r => r.RoomType)
                .Include(r => r.UpdatedUser)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Title = "Room Create";

            ViewData["HotelId"] = new SelectList(await _context.Hotels.ToListAsync(), "HotelId", "Name");
            ViewData["RoomTypeId"] = new SelectList(await _context.RoomTypes.ToListAsync(), "RoomTypeId", "RoomTypeName");
            var betTypes = await _context.BedTypes.Select(x => new { text = x.BedTypeName, value = x.BedTypeId } ).ToListAsync();

            ViewData["BedTypes"] = JsonSerializer.Serialize(betTypes);
            ViewData["RoomFacilities"] = new SelectList(await _context.FacilityTypes.ToListAsync(), "FacilityTypeId", "FacilityType");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room room)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                room.RoomId = Guid.NewGuid().ToString();
                room.CreatedUserId = user.Id;
                room.CreatedDate = DateTime.Now;

                room.RoomBedsViewModelList = JsonSerializer.Deserialize<List<RoomBedsViewModel>>(room.RoomBedsJsonString);

                if (room.RoomBedsViewModelList != null && room.RoomBedsViewModelList.Count > 0)
                {
                    List<RoomBed> roomBeds=new List<RoomBed>();

                    foreach(var item in room.RoomBedsViewModelList)
                    {
                        var roomBed = new RoomBed
                        {
                            RoomId = room.RoomId,
                            BedTypeId = item.BedTypes.FirstOrDefault().value,
                            NumberOfBeds = Convert.ToInt32(item.NumberOfBeds),
                            RoomBedId = Guid.NewGuid().ToString()
                        };

                        roomBeds.Add(roomBed);
                    }

                    room.RoomBeds = roomBeds;
                }


                if (room.FormFilePhotos.Count > 0)
                {
                    room.RoomPhotos = await GetRoomPhotos(room.FormFilePhotos, room.RoomId);
                }


                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "Name", room.HotelId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName", room.RoomTypeId);

            return View(room);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Title = "Room Update";

            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "Name", room.HotelId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName", room.RoomTypeId);

            return View(room);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Room room)
        {
            if (id != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }


            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "Name", room.HotelId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeName", room.RoomTypeId);

            return View(room);
        }


        public async Task<IActionResult> Delete(string id)
        {
            ViewBag.Title = "Room Delete";

            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.CreatedUser)
                .Include(r => r.Hotel)
                .Include(r => r.RoomType)
                .Include(r => r.UpdatedUser)
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Rooms == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Rooms'  is null.");
            }
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(string id)
        {
            return _context.Rooms.Any(e => e.RoomId == id);
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

                    photo.RoomPhotoId = Guid.NewGuid().ToString();
                    photo.RoomId = roomId;
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
