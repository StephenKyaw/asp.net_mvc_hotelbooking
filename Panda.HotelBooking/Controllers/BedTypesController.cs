using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Panda.HotelBooking.Data;
using Panda.HotelBooking.Models;

namespace Panda.HotelBooking.Controllers
{
    public class BedTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BedTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BedTypes
        public async Task<IActionResult> Index()
        {
              return View(await _context.BedTypes.ToListAsync());
        }

        // GET: BedTypes/Details/5
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

        // GET: BedTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BedTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BedTypeId,BedTypeName")] BedType bedType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bedType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bedType);
        }

        // GET: BedTypes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
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

        // POST: BedTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    _context.Update(bedType);
                    await _context.SaveChangesAsync();
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

        // GET: BedTypes/Delete/5
        public async Task<IActionResult> Delete(string id)
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

        // POST: BedTypes/Delete/5
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
