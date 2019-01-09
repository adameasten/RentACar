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
            return View(new StartPageVM());
        }
    }
}