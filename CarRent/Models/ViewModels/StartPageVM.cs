﻿using System;
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
    }
}
