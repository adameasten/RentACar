using CarRent.Models.Entities;
using CarRent.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models
{
    public class CarServices
    {
        CarRentContext context;
        public CarServices(CarRentContext context)
        {
            this.context = context;
        }

        public CarDetailsVM FindCarByID(int ID)
        {
            return context.Car
             .Where(c => c.Id == ID)
            .Select(d => new CarDetailsVM()
            {
                car = new CarVM()
                {
                    Ac = d.Ac,
                    ChildSeat = d.ChildSeat,
                    Description = d.Description,
                    Doors = d.Doors,
                    Fuel = d.Fuel,
                    Gear = d.Gear,
                    ImgUrl = d.ImgUrl,
                    Km = d.Km,
                    Model = d.Model,
                    Pets = d.Pets,
                    Price = d.Price,
                    RoofRack = d.RoofRack,
                    Seats = d.Seats,
                    TowBar = d.TowBar,
                    Type = d.Type
                },
                reviews = context.Rent
                    .Where(o => o.Car.Id == ID)
                    .SelectMany(o => o.Review.Select(r => r))
                    .ToList()
            }).FirstOrDefault();
        }
    }
}
