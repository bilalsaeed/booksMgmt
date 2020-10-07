using booksmanagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace booksmanagement.Dtos
{
    public class BookRequestDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int BookId { get; set; }
        public string Purpose { get; set; }
    }
}