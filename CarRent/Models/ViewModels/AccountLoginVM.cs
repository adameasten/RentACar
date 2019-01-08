using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.ViewModels
{
    public class AccountLoginVM
    {
        [Display(Name = "Användarnamn")]
        [Required(ErrorMessage = "Skriv in ditt användarnamn")]
        public string UserName { get; set; }

        [Display(Name = "Lösenord")]
        [Required(ErrorMessage = "Skriv in ditt lösenord")]
        public string Password { get; set; }
    }
}
