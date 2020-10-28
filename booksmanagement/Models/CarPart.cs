using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace booksmanagement.Models
{
    public class CarPart
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Car Car { get; set; }
        public int CarId { get; set; }
        public CarPartType CarPartType { get; set; }
        public int CarPartTypeId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}