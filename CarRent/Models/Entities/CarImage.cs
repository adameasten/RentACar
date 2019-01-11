using System;
using System.Collections.Generic;

namespace CarRent.Models.Entities
{
    public partial class CarImage
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string ImgUrl { get; set; }

        public virtual Car Car { get; set; }
    }
}
