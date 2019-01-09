using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.ViewModels
{
    public class ReviewCarDetailsVM
    {
        public string Review { get; set; }
        public int Rating { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserName { get; set; }
    }
}
