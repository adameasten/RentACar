using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.ViewModels
{
    public class CarRegistrationPostVM
    {
        public CarRegistrationPostVM()
        {
            CreateCarRegistrationVm();
        }

        [Required(ErrorMessage = "Ange modell")]
        [Display(Name = "Model")]
        public string Model { get; set; }
        [Required(ErrorMessage = "Ange kilometerantal")]
        [Display(Name = "Km")]
        public int Km { get; set; }

        [Display(Name = "Typ")]
        public SelectListItem[] TypeItems{ get; set; }
        public string Type { get; set; }

        [Display(Name = "Växellåda")]
        public SelectListItem[] GearItems { get; set; }
        public string Gear { get; set; }

        [Display(Name = "Drivmedel")]
        public SelectListItem[] FuelItems{ get; set; }
        public string Fuel { get; set; }

        [Display(Name = "Antal säten")]
        public SelectListItem[] SeatsItem{ get; set; }
        public int Seats { get; set; }

        [Display(Name = "Antal dörrar")]
        public SelectListItem[] DoorsItem{ get; set; }
        public int Doors { get; set; }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
        [Display(Name = "AC")]
        public bool Ac { get; set; }
        [Display(Name = "Barnsäte")]
        public bool ChildSeat { get; set; }
        [Display(Name = "Dragkrok")]
        public bool TowBar { get; set; }
        [Display(Name = "Tackräcke")]
        public bool RoofRack { get; set; }
        [Display(Name = "Husdjur")]
        public bool Pets { get; set; }
        [Required(ErrorMessage = "Ange pris per/h")]
        [Display(Name = "Pris per/h")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Ange årsmodell")]
        [Display(Name = "Årsmodell")]
        public int? YearModel { get; set; }
        [Display(Name = "Stad")]
        public string City { get; set; }
        [Display(Name = "Gata")]
        public string Street { get; set; }
        [Display(Name = "Bild")]
        public List<IFormFile> Image { get; set; }
        public string Filelocation { get; set; }



        public void CreateCarRegistrationVm()
        {
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
            };

            GearItems = new SelectListItem[]
            {
                new SelectListItem{Value = "Automat", Text = "Automat"},
                new SelectListItem{Value = "Manuell", Text = "Manuell"},
            };

            FuelItems = new SelectListItem[]
            {
                new SelectListItem{Value = "Bensin", Text = "Bensin"},
                new SelectListItem{Value = "Diesel", Text = "Diesel"},
                new SelectListItem{Value = "Etanol", Text = "Etanol"},
                new SelectListItem{Value = "El", Text = "El"},
            };

            SeatsItem = new SelectListItem[]
            {
                new SelectListItem{Value = "1", Text = "1"},
                new SelectListItem{Value = "2", Text = "2"},
                new SelectListItem{Value = "3", Text = "3"},
                new SelectListItem{Value = "4", Text = "4"},
                new SelectListItem{Value = "5", Text = "5"},
                new SelectListItem{Value = "6", Text = "6"},
                new SelectListItem{Value = "7", Text = "7+"},
            };

            DoorsItem = new SelectListItem[]
            {
                new SelectListItem{Value = "1", Text = "1"},
                new SelectListItem{Value = "2", Text = "2"},
                new SelectListItem{Value = "3", Text = "3"},
                new SelectListItem{Value = "4", Text = "4"},
                new SelectListItem{Value = "5", Text = "5"},
                new SelectListItem{Value = "6", Text = "6"}
            };

        }
    }
}
