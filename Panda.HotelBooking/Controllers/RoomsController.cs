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
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Rooms.Include(r => r.CreatedUser).Include(r => r.Hotel).Include(r => r.RoomType).Include(r => r.UpdatedUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Rooms/Details/5
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

        // GET: Rooms/Create
        public IActionResult Create()
        {
            ViewData["CreatedUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelId");
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeId");
            ViewData["UpdatedUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,HotelId,RoomTypeId,Price,Rate,NumberOfRooms")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedUserId"] = new SelectList(_context.Users, "Id", "Id", room.CreatedUserId);
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelId", room.HotelId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeId", room.RoomTypeId);
            ViewData["UpdatedUserId"] = new SelectList(_context.Users, "Id", "Id", room.UpdatedUserId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            ViewData["CreatedUserId"] = new SelectList(_context.Users, "Id", "Id", room.CreatedUserId);
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelId", room.HotelId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeId", room.RoomTypeId);
            ViewData["UpdatedUserId"] = new SelectList(_context.Users, "Id", "Id", room.UpdatedUserId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RoomId,HotelId,RoomTypeId,Price,Rate,NumberOfRooms")] Room room)
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
            ViewData["CreatedUserId"] = new SelectList(_context.Users, "Id", "Id", room.CreatedUserId);
            ViewData["HotelId"] = new SelectList(_context.Hotels, "HotelId", "HotelId", room.HotelId);
            ViewData["RoomTypeId"] = new SelectList(_context.RoomTypes, "RoomTypeId", "RoomTypeId", room.RoomTypeId);
            ViewData["UpdatedUserId"] = new SelectList(_context.Users, "Id", "Id", room.UpdatedUserId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(string id)
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

        // POST: Rooms/Delete/5
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
