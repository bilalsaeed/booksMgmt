using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace booksmanagement.Models
{
    public class CarBrand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}