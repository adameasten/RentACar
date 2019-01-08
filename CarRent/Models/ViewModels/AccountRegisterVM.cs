using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.ViewModels
{
    public class AccountRegisterVM
    {
        [Display(Name = "Användarnamn")]
        [Required(ErrorMessage = "Skriv in ditt önskade användarnamn")]
        public string UserName { get; set; }

        [Display(Name = "Lösenord")]
        [Required(ErrorMessage = "Skriv in ditt önskade lösenord")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Skriv in din E-mail")]
        [EmailAddress(ErrorMessage = "Skriv en giltlig E-mail")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }
}
