using booksmanagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace booksmanagement.Dtos
{
    public class CarTreeDto
    {
        public List<CarBrand> CarBrands { get; set; }
        public List<Car> Cars { get; set; }
        public List<CarPart> CarParts { get; set; }
        public List<CarPartComponent> CarPartComponents { get; set; }
    }
}