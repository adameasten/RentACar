using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [StringLength(maximumLength: 100, MinimumLength = 8, ErrorMessage = "Lösenordet måste vara 8-100 tecken långt")]
        [RegularExpression(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])",ErrorMessage = "Lösenordet måste innehålla minst en stor bokstav, en liten bokstav och en siffra")]
        [Required(ErrorMessage = "Skriv in ditt önskade lösenord")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Lösenorden måste överensstämma")]
        [StringLength(maximumLength: 100, MinimumLength = 8, ErrorMessage = "Lösenordet måste vara 8-100 tecken långt")]
        [RegularExpression(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])", ErrorMessage = "Lösenordet måste innehålla minst en stor bokstav, en liten bokstav och en siffra")]
        [Required(ErrorMessage = "Upprepa ditt önskade lösenord")]
        [Display(Name = "Bekräfta lösenord")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Skriv in din E-mail")]
        [EmailAddress(ErrorMessage = "Skriv en giltlig E-mail")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }
}
