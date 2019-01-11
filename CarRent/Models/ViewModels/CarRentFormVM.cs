using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.ViewModels
{
    public class CarRentFormVM
    {
        public int CarId { get; set; }
        [Display(Name = "Från")]
        public DateTime StartTime { get; set; } = DateTime.Now.Date;
        [Display(Name = "Till")]
        public DateTime EndTime { get; set; } = DateTime.Now.Date.AddDays(1);
        public decimal Price { get; set; }
    }
}
