using CarRent.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.ViewModels
{
    public class CarDetailsVM
    {
        public CarVM car { get; set; }
        public CarRentFormVM form { get; set; }
        public List<ReviewCarDetailsVM> reviews;
    }
}
