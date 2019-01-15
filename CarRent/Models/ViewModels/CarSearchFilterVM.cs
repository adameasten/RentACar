using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.ViewModels
{
    public class CarSearchFilterVM
    {
        public CarSearchVM[] SearchResult { get; set; }

        public string SearchResultJson { get; set; }

        [Display(Name = "Pris per dag")]
        public decimal Price { get; set; }

        public bool Ac { get; set; }
        [Display(Name = "Barnsäte")]
        public bool ChildSeat { get; set; }
        [Display(Name = "Dragkrok")]
        public bool TowBar { get; set; }
        [Display(Name = "Tackräcke")]
        public bool RoofRack { get; set; }
        [Display(Name = "Husdjur")]
        public bool Pets { get; set; }

        [Display(Name = "Typ")]
        public SelectListItem[] TypeItems { get; set; }
        public string Type { get; set; }

        [Display(Name = "Växellåda")]
        public SelectListItem[] GearItems { get; set; }
        public string Gear { get; set; }

        [Display(Name = "Drivmedel")]
        public SelectListItem[] FuelItems { get; set; }
        public string Fuel { get; set; }

        [Display(Name = "Minsta antal säten")]
        public SelectListItem[] SeatsItem { get; set; }
        public int Seats { get; set; }

        [Display(Name = "Minsta antal dörrar")]
        public SelectListItem[] DoorsItem { get; set; }
        public int Doors { get; set; }

    }
}
