using System;
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

        [Authorize]
        [HttpGet]
        public IActionResult CarRegistration()
        {

            return View(new CarRegistrationPostVM());

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CarRegistration(CarRegistrationPostVM vm)
        {

            if (!ModelState.IsValid)
                return View(vm);

            string userId = userManager.GetUserId(HttpContext.User);
            await services.AddCarToDatabase(vm, userId);

            return RedirectToAction("myaccount","accounts");
        }

        [HttpPost]
        public async Task<IActionResult> Search(StartPageVM vM)
        {
            var cor = await homeService.GetCoordinates(vM.City);
            homeService.AddTimeToDates(vM);
            Response.Cookies.Append("startDate", vM.StartDate.ToString());
            Response.Cookies.Append("endDate", vM.EndDate.ToString());
            var result = homeService.CompareCoords(cor, vM);

            return View(result);
        }

        [HttpGet]
        public IActionResult RentCar()
        {
            if (string.IsNullOrEmpty(Request.Cookies["DetailsId"]))
            {
                return Redirect("/");
            }
            return Redirect($"/car/details/{Request.Cookies["DetailsId"]}");
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

                return RedirectToAction(nameof(Confirmation),vM);
            }
            else
                return Content("Failure");
        }

        [HttpGet]
        public IActionResult Confirmation(CarRentFormVM vM)
        {
            var recipt = homeService.MakeRecipt(vM);

            return View(recipt);
        }

        //[HttpGet]
        //public IActionResult Confirmation()
        //{
        //    var vM = new CarRentFormVM
        //    {
        //        CarId = 1,
        //        Price = 2200,
        //        StartTime = DateTime.Now,
        //        EndTime = DateTime.Now.AddDays(2),

        //    };
        //    var recipt = homeService.MakeRecipt(vM);

        //    return View(recipt);
        //}

    }
}