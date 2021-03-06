﻿using System;
using System.Collections.Generic;

namespace CarRent.Models.Entities
{
    public partial class Review
    {
        public int Id { get; set; }
        public string Review1 { get; set; }
        public int Rating { get; set; }
        public DateTime DateCreated { get; set; }
        public int RentId { get; set; }

        public virtual Rent Rent { get; set; }
    }
}
