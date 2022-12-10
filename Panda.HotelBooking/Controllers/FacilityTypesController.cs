using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panda.HotelBooking.Data;
using Panda.HotelBooking.Models;

namespace Panda.HotelBooking.Controllers
{
    public class FacilityTypesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public FacilityTypesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Facility Type Listing";
            return View(await _context.FacilityTypes.Include(x => x.CreatedUser).ToListAsync());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.FacilityTypes == null)
            {
                return NotFound();
            }

            var facilityType = await _context.FacilityTypes
                .FirstOrDefaultAsync(m => m.FacilityTypeId == id);
            if (facilityType == null)
            {
                return NotFound();
            }

            return View(facilityType);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Facility Type Create";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FacilityTypeId,FacilityTypeName")] FacilityType facilityType)
        {
            if (ModelState.IsValid)
            {
                var user =await _userManager.GetUserAsync(User);

                facilityType.FacilityTypeId = Guid.NewGuid().ToString();
                facilityType.CreatedUserId = user.Id;
                facilityType.CreatedDate= DateTime.Now;

                _context.Add(facilityType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(facilityType);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Title = "Facility Type Update";

            if (id == null || _context.FacilityTypes == null)
            {
                return NotFound();
            }

            var facilityType = await _context.FacilityTypes.FindAsync(id);
            if (facilityType == null)
            {
                return NotFound();
            }
            return View(facilityType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FacilityTypeId,FacilityTypeName")] FacilityType facilityType)
        {
            if (id != facilityType.FacilityTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var facilityTypeUpdate = await _context.FacilityTypes.FindAsync(id);

                    if(facilityTypeUpdate != null)
                    {
                        var user = await _userManager.GetUserAsync(User);

                        facilityTypeUpdate.UpdatedUserId = user.Id;
                        facilityTypeUpdate.UpdatedDate = DateTime.Now;
                        facilityTypeUpdate.FacilityTypeName = facilityType.FacilityTypeName;

                        _context.Update(facilityTypeUpdate);
                        await _context.SaveChangesAsync();
                    }                  
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacilityTypeExists(facilityType.FacilityTypeId))
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
            return View(facilityType);
        }

        public async Task<IActionResult> Delete(string id)
        {
            ViewBag.Title = "Facility Type Delete";

            if (id == null || _context.FacilityTypes == null)
            {
                return NotFound();
            }

            var facilityType = await _context.FacilityTypes
                .FirstOrDefaultAsync(m => m.FacilityTypeId == id);
            if (facilityType == null)
            {
                return NotFound();
            }

            return View(facilityType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.FacilityTypes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.FacilityTypes'  is null.");
            }
            var facilityType = await _context.FacilityTypes.FindAsync(id);
            if (facilityType != null)
            {
                _context.FacilityTypes.Remove(facilityType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacilityTypeExists(string id)
        {
            return _context.FacilityTypes.Any(e => e.FacilityTypeId == id);
        }
    }
}
