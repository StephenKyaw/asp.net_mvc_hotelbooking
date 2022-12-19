using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebMvcUI.Models;
using Application;
using Domain;
using Domain.Interfaces;
using Domain.Dtos;

namespace WebMvcUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICityService _cityService;
        public HomeController(ILogger<HomeController> logger, ICityService cityService)
        {
            _logger = logger;
            _cityService = cityService;
        }

        public async Task<IActionResult> Index()
        {
            var dto = new CityDto() { Id = Guid.NewGuid().ToString(), Name = "City" };

            await _cityService.Insert(dto);

            var _list = await _cityService.GetAll();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}