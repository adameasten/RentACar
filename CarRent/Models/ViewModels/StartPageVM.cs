using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.ViewModels
{
    public class StartPageVM
    {
        [Required]
        [Display(Name ="Plats")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Hämtas")]
        // [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime StartDate { get; set; } = DateTime.Today;
        [Required]
        [Display(Name = "Lämnas")]
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(1);

        public string StartingHour { get; set; }
        public string EndingHour { get; set; }

        public SelectListItem[] TimeStamps { get; set; } =
        {
            new SelectListItem {Text = "12:00", Value = "12:00"},
            new SelectListItem {Text = "13:00", Value = "13:00"}
        };



        //public double Latitude { get; set; } = 59.3477632;
        //public double Longitude { get; set; } = 18.042060799999998;
        //public CarSearchVM[] nearbyCards { get; set; } = new CarSearchVM[0];
    }
}
