using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace booksmanagement.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CarBrand CarBrand { get; set; }
        public int CarBrandId { get; set; }
        public string Class { get; set; }
        public GeneralMediaFile MaintenancePlan { get; set; }
        public int? MaintenancePlanId { get; set; }

        [Display(Name = "Archived")]
        public bool IsArchived { get; set; }
        public int? DrawingOrderId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}