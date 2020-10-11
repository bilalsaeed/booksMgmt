using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace booksmanagement.Dtos
{
    public class AppUserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }

    }
}