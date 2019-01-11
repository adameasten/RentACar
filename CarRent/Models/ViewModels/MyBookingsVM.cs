using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.ViewModels
{
    public class MyBookingsVM
    {
        public string Model { get; set; }
        public string ImgUrl { get; set; }

        public int RentId { get; set; }


        [Display(Name = "Från")]
        public DateTime StartTime { get; set; }
        [Display(Name = "Till")]
        public DateTime EndTime { get; set; } 


    }
}
