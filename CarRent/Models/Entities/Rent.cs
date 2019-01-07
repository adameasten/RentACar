using System;
using System.Collections.Generic;

namespace CarRent.Models.Entities
{
    public partial class Rent
    {
        public Rent()
        {
            Review = new HashSet<Review>();
        }

        public int Id { get; set; }
        public int CarId { get; set; }
        public string CustomerId { get; set; }
        public DateTime Datestart { get; set; }
        public DateTime DateEnd { get; set; }

        public Car Car { get; set; }
        public ICollection<Review> Review { get; set; }
    }
}
