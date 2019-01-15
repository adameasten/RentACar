using GeoAPI.Geometries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.ViewModels
{
    public class EditCarVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ange modell")]
        [Display(Name = "Modell")]
        public string Model { get; set; }
        [Required(ErrorMessage = "Ange kilometerantal")]
        [Display(Name = "Km")]
        public int Km { get; set; }

        [Display(Name = "Typ")]
        public SelectListItem[] TypeItems { get; set; }
        public string Type { get; set; }

        [Display(Name = "Växellåda")]
        public SelectListItem[] GearItems { get; set; }
        public string Gear { get; set; }

        [Display(Name = "Drivmedel")]
        public SelectListItem[] FuelItems { get; set; }
        public string Fuel { get; set; }

        [Display(Name = "Antal säten")]
        public SelectListItem[] SeatsItem { get; set; }
        public int Seats { get; set; }

        [Display(Name = "Antal dörrar")]
        public SelectListItem[] DoorsItem { get; set; }
        public int Doors { get; set; }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Display(Name = "AC")]
        public bool Ac { get; set; }
        [Display(Name = "Barnsäte")]
        public bool ChildSeat { get; set; }
        [Display(Name = "Dragkrok")]
        public bool TowBar { get; set; }
        [Display(Name = "Takräcke")]
        public bool RoofRack { get; set; }
        [Display(Name = "Husdjur tillåts")]
        public bool Pets { get; set; }

        [Required(ErrorMessage = "Ange pris per/h")]
        [Display(Name = "Pris per/dag")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Ange årsmodell")]
        [Display(Name = "Årsmodell")]
        public int? YearModel { get; set; }
        
    }
}
