using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Panda.HotelBooking.Data;

namespace Panda.HotelBooking.Controllers
{
    public class HotelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HotelsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Hotel Listing";

            var applicationDbContext = _context.Hotels
                .Include(h => h.City)
                .Include(h => h.Township)
                .Include(x => x.CreatedUser);

            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .Include(h => h.City)
                .Include(h => h.Township)
                .FirstOrDefaultAsync(m => m.HotelId == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Hotel Create";

            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName");
            ViewData["TownshipId"] = new SelectList(_context.Townships, "TownshipId", "TownshipName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Address,Email,Phone_1,Phone_2,Phone_3,CityId,TownshipId")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                hotel.HotelId = Guid.NewGuid().ToString();
                hotel.CreatedUserId = user.Id;
                hotel.CreatedDate = DateTime.Now;

                _context.Add(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", hotel.CityId);
            ViewData["TownshipId"] = new SelectList(_context.Townships, "TownshipId", "TownshipName", hotel.TownshipId);

            return View(hotel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Title = "Hotel Update";

            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", hotel.CityId);
            ViewData["TownshipId"] = new SelectList(_context.Townships, "TownshipId", "TownshipName", hotel.TownshipId);
            return View(hotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("HotelId,Name,Description,Address,Email,Phone_1,Phone_2,Phone_3,CityId,TownshipId")] Hotel hotel)
        {
            if (id != hotel.HotelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var hotelUpdate = await _context.Hotels.FindAsync(hotel.HotelId);

                    if (hotelUpdate != null)
                    {
                        var user = await _userManager.GetUserAsync(User);

                        hotelUpdate.UpdatedUserId = user.Id;
                        hotelUpdate.UpdatedDate = DateTime.Now;
                        hotelUpdate.Name = hotel.Name;
                        hotelUpdate.Description = hotel.Description;
                        hotelUpdate.Address = hotel.Address;
                        hotelUpdate.Email = hotel.Email;
                        hotelUpdate.Phone_1 = hotel.Phone_1;
                        hotelUpdate.Phone_2 = hotel.Phone_2;
                        hotelUpdate.Phone_3 = hotel.Phone_3;
                        hotelUpdate.CityId = hotel.CityId;
                        hotelUpdate.TownshipId = hotel.TownshipId;


                        _context.Update(hotelUpdate);
                        await _context.SaveChangesAsync();
                    }


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.HotelId))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", hotel.CityId);
            ViewData["TownshipId"] = new SelectList(_context.Townships, "TownshipId", "TownshipName", hotel.TownshipId);
            return View(hotel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            ViewBag.Title = "Hotel Delete";

            if (id == null || _context.Hotels == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotels
                .Include(h => h.City)
                .Include(h => h.Township)
                .FirstOrDefaultAsync(m => m.HotelId == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Hotels == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Hotels'  is null.");
            }
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelExists(string id)
        {
            return _context.Hotels.Any(e => e.HotelId == id);
        }
    }
}
