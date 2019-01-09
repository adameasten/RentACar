using CarRent.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CarRent.Models
{
    public class AccountsService
    {
        UserManager<MyIdentityUser> userManager;
        SignInManager<MyIdentityUser> signInManager;
        MyIdentityContext context;

        public AccountsService(MyIdentityContext context, UserManager<MyIdentityUser> userManager, SignInManager<MyIdentityUser> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<bool> AddUser(AccountRegisterVM vm)
        {
            var result = await userManager.CreateAsync(new MyIdentityUser {UserName = vm.UserName, Email = vm.Email },vm.Password);
            return result.Succeeded;
        }

        public async Task<bool> LoginUser(AccountLoginVM vm)
        {
            var result = await signInManager.PasswordSignInAsync(vm.UserName, vm.Password,false,false);
            return result.Succeeded;
        }

        public async Task LogOutUser()
        {
            await signInManager.SignOutAsync();
        }

       
    }
}
