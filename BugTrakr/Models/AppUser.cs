using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrakr.Models
{
    public class AppUser : IdentityUser<int>
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Must enter a First Name!")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "First Name must be 3 to 30 characters long!")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Must enter a Last Name!")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Last Name must be 3 to 30 characters long!")]
        public string LastName { get; set; }
    
    }
}
