using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Panda.HotelBooking.Data;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;

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
    }
}
