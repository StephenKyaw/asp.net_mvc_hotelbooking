using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panda.HotelBooking.Data;
using Panda.HotelBooking.Models;

namespace Panda.HotelBooking.Controllers
{
    public class RoomTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoomTypesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "RoomType Listing";

              return View(await _context.RoomTypes.Include(x => x.CreatedUser).ToListAsync());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.RoomTypes == null)
            {
                return NotFound();
            }

            var roomType = await _context.RoomTypes
                .FirstOrDefaultAsync(m => m.RoomTypeId == id);
            if (roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "RoomType Create";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomTypeId,RoomTypeName")] RoomType roomType)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                roomType.RoomTypeId = Guid.NewGuid().ToString();
                roomType.CreatedUserId = user.Id;
                roomType.CreatedDate = DateTime.Now;

                _context.Add(roomType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roomType);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Title = "RoomType Update";

            if (id == null || _context.RoomTypes == null)
            {
                return NotFound();
            }

            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType == null)
            {
                return NotFound();
            }
            return View(roomType);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RoomTypeId,RoomTypeName")] RoomType roomType)
        {
            if (id != roomType.RoomTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var updateRoomType = await _context.RoomTypes.FindAsync(roomType.RoomTypeId);

                    if(updateRoomType != null)
                    {
                        var user = await _userManager.GetUserAsync(User);

                        updateRoomType.UpdatedUserId = user.Id;
                        updateRoomType.UpdatedDate = DateTime.Now;
                        updateRoomType.RoomTypeName = roomType.RoomTypeName;

                        _context.Update(updateRoomType);
                        await _context.SaveChangesAsync();
                    }

                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomTypeExists(roomType.RoomTypeId))
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
            return View(roomType);
        }

        public async Task<IActionResult> Delete(string id)
        {
            ViewBag.Title = "RoomType Delete";

            if (id == null || _context.RoomTypes == null)
            {
                return NotFound();
            }

            var roomType = await _context.RoomTypes
                .FirstOrDefaultAsync(m => m.RoomTypeId == id);
            if (roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.RoomTypes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.RoomTypes'  is null.");
            }
            var roomType = await _context.RoomTypes.FindAsync(id);
            if (roomType != null)
            {
                _context.RoomTypes.Remove(roomType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomTypeExists(string id)
        {
          return _context.RoomTypes.Any(e => e.RoomTypeId == id);
        }
    }
}
