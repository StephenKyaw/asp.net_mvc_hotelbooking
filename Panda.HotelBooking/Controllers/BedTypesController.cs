using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panda.HotelBooking.Data;
using Panda.HotelBooking.Models;

namespace Panda.HotelBooking.Controllers
{
    public class BedTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BedTypesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "BetType Listing";
            return View(await _context.BedTypes.Include(x => x.CreatedUser).ToListAsync());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.BedTypes == null)
            {
                return NotFound();
            }

            var bedType = await _context.BedTypes
                .FirstOrDefaultAsync(m => m.BedTypeId == id);
            if (bedType == null)
            {
                return NotFound();
            }

            return View(bedType);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "BetType Create";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BedTypeId,BedTypeName")] BedType bedType)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                bedType.BedTypeId = Guid.NewGuid().ToString();
                bedType.CreatedUserId = user.Id;
                bedType.CreatedDate = DateTime.Now;

                _context.Add(bedType);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(bedType);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Title = "BetType Update";

            if (id == null || _context.BedTypes == null)
            {
                return NotFound();
            }

            var bedType = await _context.BedTypes.FindAsync(id);
            if (bedType == null)
            {
                return NotFound();
            }
            return View(bedType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BedTypeId,BedTypeName")] BedType bedType)
        {
            if (id != bedType.BedTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var bedTypeUpdate = await _context.BedTypes.FindAsync(id);

                    if (bedTypeUpdate != null)
                    {
                        var user = await _userManager.GetUserAsync(User);

                        bedTypeUpdate.UpdatedUserId = user.Id;
                        bedTypeUpdate.UpdatedDate = DateTime.Now;
                        bedTypeUpdate.BedTypeName = bedType.BedTypeName;

                        _context.Update(bedTypeUpdate);
                        await _context.SaveChangesAsync();
                    }


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BedTypeExists(bedType.BedTypeId))
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
            return View(bedType);
        }

        public async Task<IActionResult> Delete(string id)
        {
            ViewBag.Title = "BetType Delete";

            if (id == null || _context.BedTypes == null)
            {
                return NotFound();
            }

            var bedType = await _context.BedTypes
                .FirstOrDefaultAsync(m => m.BedTypeId == id);
            if (bedType == null)
            {
                return NotFound();
            }

            return View(bedType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.BedTypes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BedTypes'  is null.");
            }
            var bedType = await _context.BedTypes.FindAsync(id);
            if (bedType != null)
            {
                _context.BedTypes.Remove(bedType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BedTypeExists(string id)
        {
            return _context.BedTypes.Any(e => e.BedTypeId == id);
        }
    }
}
