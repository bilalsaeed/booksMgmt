using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace booksmanagement.Models
{
    public class BookBorrowOrder
    {
        public int Id { get; set; }
        public string Purpose { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int Status { get; set; } //1=> Pending, 2=> Approved, 3=> Granted, 4=> Returned
        public ApplicationUser Applicant { get; set; }
        public string ApplicantId { get; set; }
    }
}