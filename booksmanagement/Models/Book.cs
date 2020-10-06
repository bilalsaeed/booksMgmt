using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace booksmanagement.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        
        public Car Car { get; set; }

        [Required]
        public int CarId { get; set; }

        [Required]
        [Display(Name = "Book type")]
        public int TypeId { get; set; } //1 => Softcopy, 2 => Hardcopy
        public int? Quantity { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Created date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}