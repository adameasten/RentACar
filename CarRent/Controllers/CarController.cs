using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Models;
using CarRent.Models.ViewModels;
using GeoAPI.Geometries;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Controllers
{
    public class CarController : Controller
    {
        CarServices services;
        HomeService homeService;
        public CarController(CarServices services, HomeService homeService)
        {
            this.services = services;
            this.homeService = homeService;
            
        }
    
        public IActionResult Details(int ID)
        {
            var model = services.FindCarByID(ID);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Search(StartPageVM vM)
        {

            var cor = await homeService.GetCoordinates(vM);
            homeService.AddTimeToDates(vM);

            var result = homeService.CompareCoords(cor,vM);

            return View(result);
        }
    }
}