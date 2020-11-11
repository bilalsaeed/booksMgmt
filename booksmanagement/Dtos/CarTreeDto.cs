using booksmanagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace booksmanagement.Dtos
{
    public class CarTreeDto
    {
        //public List<CarBrand> CarBrands { get; set; }
        //public List<Car> Cars { get; set; }
        //public List<CarPart> CarParts { get; set; }
        //public List<CarPartComponent> CarPartComponents { get; set; }
        public List<CarBrandDto> CarBrands { get; set; }
    }

    public class CarTreeBookDto
    {
        //public List<CarBrand> CarBrands { get; set; }
        //public List<Car> Cars { get; set; }
        //public List<CarPart> CarParts { get; set; }
        //public List<CarPartComponent> CarPartComponents { get; set; }
        public List<CarBrandBookDto> CarBrands { get; set; }
    }

    public class CarBrandDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool collapsed { get; set; }
        public List<CarDto> childerns { get; set; }
    }
    public class CarBrandBookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool collapsed { get; set; }
        public List<CarBookDto> childerns { get; set; }
    }
    public class CarDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public bool collapsed { get; set; }
        public bool bookAvailable { get; set; }
        public bool carPart { get; set; }
        public List<CarPartTypeDto> childerns { get; set; }
    }
    public class CarBookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public bool collapsed { get; set; }
        public bool bookAvailable { get; set; }
        public bool softCopy { get; set; }
        public int bookId { get; set; }
        public int softBookId { get; set; }
        public bool car { get; set; }
        public List<CarPartDto> childerns { get; set; }
    }
    public class CarPartTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool collapsed { get; set; }
        public List<CarPartDto> childerns { get; set; }
    }
    public class CarPartDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CarPartType CarPartType { get; set; }
        public int CarPartTypeId { get; set; }
        public int CarId { get; set; }
        public bool collapsed { get; set; }
        public bool carPart { get; set; }
        public bool bookAvailable { get; set; }
        public bool softCopy { get; set; }
        public int bookId { get; set; }
        public int softBookId { get; set; }
        public int? DrawingFileId { get; set; }
        public List<CarPartComponentDto> childerns { get; set; }
    }
    public class CarPartComponentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CarPartId { get; set; }
        public bool carPartComp { get; set; }
        public bool collapsed { get; set; }
        public bool bookAvailable { get; set; }
        public bool softCopy { get; set; }
        public int bookId { get; set; }
        public int softBookId { get; set; }
        public int? DrawingFileId { get; set; }
        public List<CarPartComponentDescDto> childerns { get; set; }
    }

    public class CarPartComponentDescDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CarPartComponentId { get; set; }
        public bool carPartCompDesc { get; set; }
        public bool bookAvailable { get; set; }
        public bool softCopy { get; set; }
        public int bookId { get; set; }
        public int softBookId { get; set; }
    }
}