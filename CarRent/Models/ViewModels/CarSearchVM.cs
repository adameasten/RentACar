using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.ViewModels
{
    public class CarSearchVM
    {
        public string ImgUrl { get; set; }
        public string Model { get; set; }
        public int? YearModel { get; set; }
        public double? Rating { get; set; } = 0;
        public decimal Price { get; set; }

        public int[] Ratings { get; set; }
    }
}
