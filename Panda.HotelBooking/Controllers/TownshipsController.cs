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

        public TownshipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Townships
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Townships.Include(t => t.City);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Townships/Details/5
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

        // GET: Townships/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName");
            return View();
        }

        // POST: Townships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TownshipName,CityId")] Township township)
        {
            if (ModelState.IsValid)
            {
                township.TownshipId = Guid.NewGuid().ToString();
                _context.Add(township);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityName", township.CityId);
            return View(township);
        }

        // GET: Townships/Edit/5
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

        // POST: Townships/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    _context.Update(township);
                    await _context.SaveChangesAsync();
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

        // GET: Townships/Delete/5
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

        // POST: Townships/Delete/5
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
