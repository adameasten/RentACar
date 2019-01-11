using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.ViewModels
{
    public class MyAccountVM
    {

       

        public AccountRegisterVM UserInfo { get; set; }

        [Display(Name = "Gammalt lösenord")]
        public string OldPassword { get; set; }

        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }
        [Display(Name = "Telefonnummer +46")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }
        [Display(Name = "Gata")]
        public string Street { get; set; }
        [Display(Name = "Stad")]
        public string City { get; set; }
        [Display(Name = "Postnummer")]
        public string Zip { get; set; }
        [Display(Name = "Personnummer")]
        public string SSN { get; set; }
        public DateTime DateJoined { get; set; } = DateTime.Now;

        public List<MyCarCard> CarCards { get; set; }
        public List<MyBookingsVM> MyBookings { get; set; }


    }
}
