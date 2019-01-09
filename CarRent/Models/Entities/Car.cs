using System;
using System.Collections.Generic;
using GeoAPI.Geometries;

namespace CarRent.Models.Entities
{
    public partial class Car
    {
        public Car()
        {
            Rent = new HashSet<Rent>();
        }

        public int Id { get; set; }
        public string OwnerId { get; set; }
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
        public IGeometry GeoLocation { get; set; }
        public int? YearModel { get; set; }


        public virtual ICollection<Rent> Rent { get; set; }
    }
}
