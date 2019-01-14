using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Models;
using CarRent.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Controllers
{
    public class AccountsController : Controller
    {
        AccountsService service;
        UserManager<MyIdentityUser> userManager;

        public AccountsController(UserManager<MyIdentityUser> userManager, AccountsService service)
        {
            this.userManager = userManager;

            this.service = service;
        }

        [HttpGet]
        public IActionResult MyAccount()
        {
            string userId = userManager.GetUserId(HttpContext.User);
            //var user = Membership.GetUser().Email;
            var user = userManager.FindByIdAsync(userManager.GetUserId(HttpContext.User)).Result;
            MyAccountVM vm = service.GetUserByID(user);

            //MyAccountVM vm = service.GetUserByID(userId);
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> MyAccount(MyAccountVM vm)
        {

            var user = userManager.FindByIdAsync(userManager.GetUserId(HttpContext.User)).Result;

            await service.UpdateUser(user, vm);

            return RedirectToAction(nameof(MyAccount));
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginVM vm, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(vm);
            var succeded = await service.LoginUser(vm);
            if (!succeded)
                return View(vm);
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            else
                return Redirect("/");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult SaveComment(string comment, int rating, int rentId)
        {
            service.AddReview(comment, rating, rentId);
            return Json(new { status = "Success" });
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            var succeded = await service.AddUser(vm);
            if (!succeded)
                return View(vm);
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await service.LogOutUser();
            return Redirect("/");
        }
    }
}