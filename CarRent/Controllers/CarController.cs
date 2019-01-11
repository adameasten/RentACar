﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Models;
using GeoAPI.Geometries;
using CarRent.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace CarRent.Controllers
{
    public class CarController : Controller
    {
        CarServices services;
        HomeService homeService;
        UserManager<MyIdentityUser> userManager;
        public CarController(CarServices services, UserManager<MyIdentityUser> userManager, HomeService homeService)
        {
            this.services = services;
            this.homeService = homeService;

            this.userManager = userManager;
        }

        public IActionResult Details(int ID)
        {
            Response.Cookies.Append("DetailsId", ID.ToString());
            var model = services.FindCarByID(ID);

            return View(model);

        }

        [HttpGet]
        public IActionResult CarRegistration()
        {

            return View(new CarRegistrationPostVM());

        }

        [HttpPost]
        public async Task<IActionResult> CarRegistration(CarRegistrationPostVM vm)
        {

            if (!ModelState.IsValid)
                return View(vm);

            string userId = userManager.GetUserId(HttpContext.User);
            await services.AddCarToDatabase(vm, userId);

            return Content("Lyckad");

        }
        [HttpPost]
        public async Task<IActionResult> Search(StartPageVM vM)
        {

            var cor = await homeService.GetCoordinates(vM.City);
            homeService.AddTimeToDates(vM);

            var result = homeService.CompareCoords(cor, vM);

            return View(result);
        }

        [HttpGet]
        public IActionResult RentCar()
        {
            return Redirect("/car/details/"+Request.Cookies["DetailsId"]);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RentCar(CarRentFormVM vM)
        {
            //if(!ModelState.IsValid)
            //    return View(vM);
            if (homeService.CarIsAvailable(vM))
            {
                string userId = userManager.GetUserId(HttpContext.User);

                await services.AddRent(vM, userId);

                return Content("Success");
            }
            else
                return Content("Failure");
        }


    }
}