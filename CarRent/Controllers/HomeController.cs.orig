using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Models;
using CarRent.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Controllers
{
    public class HomeController : Controller
    {
        HomeService service;

        public HomeController(HomeService service)
        {
            this.service = service;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Home()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(StartPageVM vM)
        {
            var cor = await service.GetCoordinates(vM);

            var result = service.CompareCoords(cor);

            return RedirectToAction(nameof(Home));
        }
    }
}