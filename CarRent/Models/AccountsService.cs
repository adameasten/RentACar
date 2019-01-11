﻿using CarRent.Models.Entities;
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
        CarRentContext carContext;

        public AccountsService(CarRentContext carContext, MyIdentityContext context, UserManager<MyIdentityUser> userManager, SignInManager<MyIdentityUser> signInManager)
        {
            this.context = context;
            this.carContext = carContext;

            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<bool> AddUser(AccountRegisterVM vm)
        {
            var result = await userManager.CreateAsync(new MyIdentityUser {UserName = vm.UserName, Email = vm.Email },vm.Password);
            return result.Succeeded;
        }

        public MyAccountVM GetUserByID(MyIdentityUser user)
        {



            return new MyAccountVM()
            {
                City = user.City,
                DateJoined = user.DateJoined,
                FirstName = user.FirstName,
                PhoneNumber = user.PhoneNumber,
                LastName = user.LastName,
                SSN = user.SSN,
                Street = user.Street,
                Zip = user.Zip,
                UserInfo = new AccountRegisterVM()
                {
                    Email = user.Email,
                    UserName = user.UserName,
                },
                CarCards = carContext.Car
                .Where(c => c.OwnerId == user.Id)
                .Select(p => new MyCarCard()
                {
                    ImgUrl = p.ImgUrl,
                    Model = p.Model
                }).ToList(),
                MyBookings = carContext.Rent
                .Where(c => c.CustomerId == user.Id).
                Select(p => new MyBookingsVM()
                {
                    ImgUrl = p.Car.ImgUrl,
                    Model = p.Car.Model,
                    StartTime = p.Datestart,
                    EndTime = p.DateEnd
                }).ToList()
            };
        }

        public async Task UpdateUser(MyIdentityUser user, MyAccountVM vm)
        {

            user.City = vm.City;
            user.Email = vm.UserInfo.Email;
            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            user.UserName = vm.UserInfo.UserName;
            user.Zip = vm.Zip;
            user.Street = vm.Street;
            user.SSN = vm.SSN;
            user.PhoneNumber = vm.PhoneNumber;

            if (vm.OldPassword!=null)
            {
                await userManager.ChangePasswordAsync(user, vm.OldPassword, vm.UserInfo.Password);

            }

            await userManager.UpdateAsync(user);
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
