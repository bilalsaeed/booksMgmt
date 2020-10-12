﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace booksmanagement.Models
{
    public class CarPartComponent
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public CarPart CarPart { get; set; }
        [Required]
        public int CarPartId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}