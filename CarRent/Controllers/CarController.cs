using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Controllers
{
    public class CarController : Controller
    {
        CarServices services;
        public CarController(CarServices services)
        {
            this.services = services;
        }
    
        public IActionResult Details(int ID)
        {
            var model = services.FindCarByID(ID);

            return View(model);
           

        }
    }
}