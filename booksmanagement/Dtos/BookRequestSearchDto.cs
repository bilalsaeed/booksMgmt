using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace booksmanagement.Dtos
{
    public class BookRequestSearchDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool ExceedDueDate { get; set; }
    }
}