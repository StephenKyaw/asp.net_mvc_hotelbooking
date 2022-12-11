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
                .Include(x => x.CreatedUser)
                .Include(x => x.HotelPhotos);

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
        public async Task<IActionResult> Create([Bind("Name,Description,Address,Email,Phone_1,Phone_2,Phone_3,CityId,TownshipId,FormFiles")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                hotel.HotelId = Guid.NewGuid().ToString();
                hotel.CreatedUserId = user.Id;
                hotel.CreatedDate = DateTime.Now;

                if (hotel.FormFiles.Count > 0)
                {
                    hotel.HotelPhotos = await GetHotelPhotos(hotel.FormFiles, hotel.HotelId);
                }


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

            var hotel = await _context.Hotels.Include(x => x.HotelPhotos).FirstOrDefaultAsync(x => x.HotelId == id);

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
        public async Task<IActionResult> Edit(string id, [Bind("HotelId,Name,Description,Address,Email,Phone_1,Phone_2,Phone_3,CityId,TownshipId,FormFiles")] Hotel hotel)
        {
            if (id != hotel.HotelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var hotelUpdate = await _context.Hotels.Include(x => x.HotelPhotos).FirstOrDefaultAsync(x => x.HotelId == hotel.HotelId);

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

                        if (hotel.FormFiles.Count > 0)
                        {
                            if (hotelUpdate.HotelPhotos.Count > 0)
                            {
                                RemoveImages(hotelUpdate.HotelPhotos.ToList());

                                _context.HotelPhotos.RemoveRange(hotelUpdate.HotelPhotos.ToList());

                            }

                            hotelUpdate.HotelPhotos = await GetHotelPhotos(hotel.FormFiles, hotel.HotelId);

                        }
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
            var hotel = await _context.Hotels.Include(x => x.HotelPhotos).FirstOrDefaultAsync(x => x.HotelId == id);
            if (hotel != null)
            {
                if (hotel.HotelPhotos.Count > 0)
                {
                    RemoveImages(hotel.HotelPhotos.ToList());
                    _context.HotelPhotos.RemoveRange(hotel.HotelPhotos);
                }
                _context.Hotels.Remove(hotel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelExists(string id)
        {
            return _context.Hotels.Any(e => e.HotelId == id);
        }

        private void RemoveImages(List<HotelPhoto> photos)
        {
            foreach (var photo in photos)
            {
                string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Hotels");

                string _fullPath = Path.Combine(_folderPath, photo.FileName);

                if (System.IO.File.Exists(_fullPath))
                {
                    System.IO.File.Delete(_fullPath);
                }
            }
        }

        private async Task<List<HotelPhoto>> GetHotelPhotos(IFormFileCollection files, string hotelId)
        {
            List<HotelPhoto> photos = new List<HotelPhoto>();

            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    HotelPhoto photo = new HotelPhoto();

                    string _folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Hotels");

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

                    photo.HotelPhotoId = Guid.NewGuid().ToString();
                    photo.HotelId = hotelId;
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
