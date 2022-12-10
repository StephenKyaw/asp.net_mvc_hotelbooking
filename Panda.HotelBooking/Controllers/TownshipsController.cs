using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Panda.HotelBooking.Data;
using Panda.HotelBooking.Models;

namespace Panda.HotelBooking.Controllers
{
    public class TownshipsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TownshipsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Townships
                .Include(t => t.City)
                .Include(x => x.CreatedUser);

            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Townships == null)
            {
                return NotFound();
            }

            var township = await _context.Townships
                .Include(t => t.City)
                .FirstOrDefaultAsync(m => m.TownshipId == id);
            if (township == null)
            {
                return NotFound();
            }

            return View(township);
        }

        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TownshipName,CityId")] Township township)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                township.TownshipId = Guid.NewGuid().ToString();
                township.CreatedUserId = user.Id;
                township.CreatedDate = DateTime.Now;

                _context.Add(township);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", township.CityId);

            return View(township);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Townships == null)
            {
                return NotFound();
            }

            var township = await _context.Townships.FindAsync(id);
            if (township == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", township.CityId);
            return View(township);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TownshipId,TownshipName,CityId")] Township township)
        {
            if (id != township.TownshipId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var updateTownShip = await _context.Townships.FindAsync(township.TownshipId);

                    if (updateTownShip != null)
                    {
                        var user = await _userManager.GetUserAsync(User);

                        updateTownShip.TownshipName = township.TownshipName;
                        updateTownShip.CityId = township.CityId;
                        updateTownShip.UpdatedUserId = user.Id;
                        updateTownShip.UpdatedDate = DateTime.Now;

                        _context.Update(updateTownShip);
                        await _context.SaveChangesAsync();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TownshipExists(township.TownshipId))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", township.CityId);
            return View(township);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Townships == null)
            {
                return NotFound();
            }

            var township = await _context.Townships
                .Include(t => t.City)
                .FirstOrDefaultAsync(m => m.TownshipId == id);
            if (township == null)
            {
                return NotFound();
            }

            return View(township);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Townships == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Townships'  is null.");
            }
            var township = await _context.Townships.FindAsync(id);
            if (township != null)
            {
                _context.Townships.Remove(township);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TownshipExists(string id)
        {
            return _context.Townships.Any(e => e.TownshipId == id);
        }
    }
}
