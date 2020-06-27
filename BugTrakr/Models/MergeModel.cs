using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrakr.Models
{
    [NotMapped]
    public class MergeModel
    {
        public MergeModel()
        {
            UserNames = new List<string>();
            UserFullNames = new List<string>();
            Users = new List<AppUser>();
            Comments = new List<Comment>();
        }

        public List<AppUser> Users { get; set; }

        public AppUser AppUsers { get; set; }

        [Display(Name ="User Name")]
        [StringLength(12,MinimumLength =5,
            ErrorMessage ="User name must be 5 to 12 characters long!")]
        [RegularExpression("^[a-zA-Z0-9]*$",ErrorMessage ="User name cannot contain any special characters")]
        public string AppUserName { get; set; }

        [Display(Name ="Email")]
        [EmailAddress(ErrorMessage ="Must be in the format of an email address")]
        public string AppUserEmail { get; set; }

        [Display(Name ="Password")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$",
            ErrorMessage ="Must contain at least: 6 characters, 1 lowercase letter, " +
            "1 upper case letter and 1 non alpha numeric character!")]
        public string AppUserPassword { get; set; }

        public List<string> UserNames { get; set; }

        public List<string> UserFullNames { get; set; }

        public int RoleId { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        [StringLength(15,MinimumLength =2,ErrorMessage ="Role Name must be 2 to 15 characters long!")]
        [RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Role Name can only contain alphabetical characters")]
        public string RoleName { get; set; }

        public Bug Bugs { get; set; }

        public List<Comment> Comments { get; set; }

        public Comment Comment { get; set; }

        public int ApprovalStatusCount { get; set; }

        public int InProgressStatusCount { get; set; }

        public int DoneStatusCount { get; set; }

        public int CompletedStatusCount { get; set; }
    }
}
