using Antlr.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Display(Name = "Download link")]
        public string DownloadLink { get; set; }

        public Car Car { get; set; }

        [Required]
        [Index("IX_Car_CarPart_CarPartComp_CarPartCompDesc", 1, IsUnique = true)]
        public int CarId { get; set; }

        public CarPart CarPart { get; set; }

        [Index("IX_Car_CarPart_CarPartComp_CarPartCompDesc", 2, IsUnique = true)]
        public int? CarPartId { get; set; }
        public CarPartComponent CarPartComponent { get; set; }

        [Index("IX_Car_CarPart_CarPartComp_CarPartCompDesc", 3, IsUnique = true)]
        public int? CarPartComponentId { get; set; }
        public CarPartComponentDesc CarPartComponentDesc { get; set; }

        [Index("IX_Car_CarPart_CarPartComp_CarPartCompDesc", 4, IsUnique = true)]
        public int? CarPartComponentDescId { get; set; }

        public string BookLocation { get; set; }
        public string BookNumber { get; set; }
        public bool PartCodeAvailable { get; set; }
        public bool SoftCopyAvailable { get; set; }

        [Required]
        [Display(Name = "Book type")]
        [Index("IX_Car_CarPart_CarPartComp_CarPartCompDesc", 5, IsUnique = true)]
        public int TypeId { get; set; } //1 => Softcopy, 2 => Hardcopy
        public int? Quantity { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        
        [Display(Name = "Created date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}