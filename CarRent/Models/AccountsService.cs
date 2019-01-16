using CarRent.Models.Entities;
using CarRent.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        CarServices service;

        public AccountsService(CarRentContext carContext, MyIdentityContext context, UserManager<MyIdentityUser> userManager, SignInManager<MyIdentityUser> signInManager, CarServices service)
        {
            this.context = context;
            this.carContext = carContext;
            this.service = service;
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
                    Id = p.Id,
                    ImgUrl = p.CarImage.Where(c => c.CarId == p.Id).Select(d => d.ImgUrl).FirstOrDefault(),
                    Model = p.Model
                }).ToList(),
                MyBookings = carContext.Rent
                .Where(c => c.CustomerId == user.Id).
                Select(p => new MyBookingsVM()
                {
                    ImgUrl = p.Car.CarImage.Where(c => c.CarId == p.Id).Select(d => d.ImgUrl).FirstOrDefault(),
                    Model = p.Car.Model,
                    StartTime = p.Datestart,
                    EndTime = p.DateEnd,
                    RentId = p.Id

                }).ToList(),
                MyReviews = carContext.Car
                .Where(r => r.OwnerId == user.Id)
                .SelectMany(r => r.Rent)
                .SelectMany(r => r.Review)
                .Select(r => new ReviewCarDetailsVM
                {
                    Review = r.Review1,
                    Rating = r.Rating,
                    DateCreated = r.DateCreated,
                    UserName = service.GetContactByID(r.Rent.CustomerId),
                    Car = r.Rent.Car.Model,

                })
                .ToList()
            };
        }

        public void AddReview(string comment, int rating, int rentId)
        {
            carContext.Review.Add(
                new Review()
                {
                    DateCreated = DateTime.Now,
                    Rating = rating,
                    RentId = rentId,
                    Review1 = comment

                });
            carContext.SaveChanges();
        }

        public void EditCar(EditCarVM vm)
        {

            var car = carContext.Car.SingleOrDefault(c => c.Id == vm.Id);
            
            car.Ac = vm.Ac;
            car.ChildSeat = vm.ChildSeat;
            car.Description = vm.Description;
            car.Doors = vm.Doors;
            car.Fuel = vm.Fuel;
            car.Gear = vm.Gear;
            car.Km = vm.Km;
            car.Model = vm.Model;
            car.Pets = vm.Pets;
            car.Price = vm.Price;
            car.RoofRack = vm.RoofRack;
            car.Seats = vm.Seats;
            car.TowBar = vm.TowBar;
            car.Type = vm.Type;
            car.YearModel = vm.YearModel;

            carContext.SaveChanges();
        }

        internal void DeleteCar(EditCarVM vm)
        {
            var car = carContext.Car.SingleOrDefault(c => c.Id == vm.Id);
            var rents = carContext.Rent.Where(r => r.CarId == car.Id);
            var reviews = carContext.Review.Where(r => rents.Select(q => q.Id).Contains(r.Id));
            var carImages = carContext.CarImage.Where(i => i.CarId == car.Id);

            foreach (var review in reviews)
            {
                carContext.Review.Remove(review);
            }

            foreach (var rent in rents)
            {
                carContext.Rent.Remove(rent);
            }

            foreach (var carImage in carImages)
            {
                carContext.CarImage.Remove(carImage);
            }

            carContext.Car.Remove(car);
            carContext.SaveChanges();
        }

        public EditCarVM FindCarById(int id)
        {
            var car = carContext.Car
                .SingleOrDefault(c => c.Id == id);

            return new EditCarVM
            {
                Id = car.Id,
                Model = car.Model,
                Description = car.Description,
                Doors = car.Doors,
                Fuel = car.Fuel,
                Gear = car.Gear,
                Km = car.Km,
                Price = car.Price,
                Seats = car.Seats,
                Type = car.Type,
                Pets = (bool)car.Pets,
                Ac = (bool)car.Ac,
                ChildSeat = (bool)car.ChildSeat,
                RoofRack = (bool)car.RoofRack,
                TowBar = (bool)car.TowBar,
                YearModel = car.YearModel,
                
                TypeItems = new SelectListItem[]
            {
                   new SelectListItem{Value = "Sedan", Text = "Sedan"},
                   new SelectListItem{Value = "Kombi", Text = "Kombi"},
                   new SelectListItem{Value = "SUV", Text = "SUV"},
                   new SelectListItem{Value = "Halvkombi", Text = "Halvkombi"},
                   new SelectListItem{Value = "Sportkupé", Text = "Sportkupé"},
                   new SelectListItem{Value = "Cab", Text = "Cab"},
                   new SelectListItem{Value = "Pickup", Text = "Pickup"},
                   new SelectListItem{Value = "Minibuss", Text = "Minibuss"},
                   new SelectListItem{Value = "Husbil", Text = "Husbil"}
            },

            GearItems = new SelectListItem[]
            {
                new SelectListItem{Value = "Automat", Text = "Automat"},
                new SelectListItem{Value = "Manuell", Text = "Manuell"},
            },

            FuelItems = new SelectListItem[]
            {
                new SelectListItem{Value = "Bensin", Text = "Bensin"},
                new SelectListItem{Value = "Diesel", Text = "Diesel"},
                new SelectListItem{Value = "Etanol", Text = "Etanol"},
                new SelectListItem{Value = "El", Text = "El"},
            },

            SeatsItem = new SelectListItem[]
            {
                new SelectListItem{Value = "1", Text = "1"},
                new SelectListItem{Value = "2", Text = "2"},
                new SelectListItem{Value = "3", Text = "3"},
                new SelectListItem{Value = "4", Text = "4"},
                new SelectListItem{Value = "5", Text = "5"},
                new SelectListItem{Value = "6", Text = "6"},
                new SelectListItem{Value = "7", Text = "7+"},
            },

            DoorsItem = new SelectListItem[]
            {
                new SelectListItem{Value = "1", Text = "1"},
                new SelectListItem{Value = "2", Text = "2"},
                new SelectListItem{Value = "3", Text = "3"},
                new SelectListItem{Value = "4", Text = "4"},
                new SelectListItem{Value = "5", Text = "5"},
                new SelectListItem{Value = "6", Text = "6"}
            },

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
