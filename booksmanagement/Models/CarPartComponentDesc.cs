using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace booksmanagement.Models
{
    public class CarPartComponentDesc
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public CarPartComponent CarPartComponent { get; set; }
        [Required]
        public int CarPartComponentId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}