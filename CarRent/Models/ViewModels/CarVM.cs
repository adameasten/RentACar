using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.ViewModels
{
    public class CarVM
    {
        public string Model { get; set; }
        public int Km { get; set; }
        public string Type { get; set; }
        public string Gear { get; set; }
        public string Fuel { get; set; }
        public int Seats { get; set; }
        public int Doors { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }
        public bool? Ac { get; set; }
        public bool? ChildSeat { get; set; }
        public bool? TowBar { get; set; }
        public bool? RoofRack { get; set; }
        public bool? Pets { get; set; }
        public decimal Price { get; set; }
        public List<string> ImgUrlArr { get; set; }
    }
}
