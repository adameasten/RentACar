using CarRent.Models.Entities;
using CarRent.Models.ViewModels;
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

            var car = context.Car.SingleOrDefault(c => c.Id == ID);

            CarDetailsVM CarModel = new CarDetailsVM() {
                car = car,
                form = new CarRentFormVM()
                {
                    Price = car.Price
                }
            };

            return CarModel;          
             

        }
    }
}
