using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace booksmanagement.ViewModels
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Pasword")]
        public string Pasword { get; set; }

        public string Phone { get; set; }

        [Display(Name = "Contact number")]
        public string ContactNumber { get; set; }

        [Display(Name = "Service number")]
        public string ServiceNumber { get; set; }

        [Display(Name = "Department")]
        public string Department { get; set; }

        [Required]
        [Display(Name = "User Role")]
        public string Role { get; set; }

        [Display(Name = "User is active")]
        public bool IsActive { get; set; }
        [Display(Name = "User is approved")]
        public bool IsApproved { get; set; }

    }
}