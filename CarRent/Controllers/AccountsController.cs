using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Models;
using CarRent.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Controllers
{
    public class AccountsController : Controller
    {
        AccountsService service;

        public AccountsController(AccountsService service)
        {
            this.service = service;
        }

        public IActionResult MyAccount()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            var succeded = await service.LoginUser(vm);
            if (!succeded)
                return View(vm);
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
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