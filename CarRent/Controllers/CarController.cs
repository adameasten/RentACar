using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Models;
using CarRent.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRent.Controllers
{
    public class CarController : Controller
    {
        CarServices services;
        UserManager<MyIdentityUser> userManager;
        public CarController(CarServices services, UserManager<MyIdentityUser> userManager)
        {
            this.services = services;
            this.userManager = userManager;
        }
    
        public IActionResult Details(int ID)
        {
            var model = services.FindCarByID(ID);

            return View(model);          

        }

        [HttpGet]
        public IActionResult CarRegistration()
        {

            return View(new CarRegistrationPostVM());

        }

        [HttpPost]
        public IActionResult CarRegistration(CarRegistrationPostVM vm)
        {

            if(!ModelState.IsValid)
                 return View(vm);

            string userId = userManager.GetUserId(HttpContext.User);
            services.AddCarToDatabase(vm, userId);

            return View();

        }


    }
}