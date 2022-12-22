using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebMvcUI.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            LocationViewModel model = new LocationViewModel();
            var _townships = new List<LocationViewModel.Townships>()
            {
                new LocationViewModel.Townships("Kamayut"),
                new LocationViewModel.Townships("Inseing")
            };
            model.TownshipsJoson = JsonSerializer.Serialize<List<LocationViewModel.Townships>>(_townships);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(LocationViewModel model)
        {
            if (ModelState.IsValid)
            {
                //LocationDto location = new LocationDto(model.CityName,
                //    "Admin",
                //    JsonSerializer.Deserialize<List<LocationViewModel.Townships>>(model.TownshipsJoson)
                //    .Select(x => x.Township).ToList()
                //    );

                //await _locationService.AddLocation(location);

            }
            return View(model);
        }
    }
}
