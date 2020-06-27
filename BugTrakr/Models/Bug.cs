using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrakr.Models
{
    public class Bug
    {
        public int BugId { get; set; }

        [Display(Name ="Title")]
        [Required(ErrorMessage = "Must enter a title for the Bug!")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Title must be 10 to 100 characters long!")]
        public string BugTitle { get; set; }

        [Required(ErrorMessage = "Must set a status for the Bug!")]
        [Display(Name = "Status")]
        public BugStatusEnum BugStatus { get; set; }

        [Display(Name = "Assigned To")]
        [Required(ErrorMessage = "A user needs to be assigned to this Bug!")]
        public int AssignedToId { get; set; }

        [Display(Name = "Assigned By")]
        [Required]
        public int AssignedById { get; set; }

        [Display(Name ="Description")]
        [Required(ErrorMessage = "Must enter information about the Bug!")]
        [StringLength(int.MaxValue, MinimumLength = 10, ErrorMessage = "The Bug Description cannot be less than 10 characters long!")]
        public string BugDescription { get; set; }

        [Required(ErrorMessage = "Must provide a diffuclty setting for the Bug!")]
        [Display(Name = "Difficulty")]
        public BugDifficultyEnum BugDifficulty { get; set; }

        public bool IsComplete { get; set; }

        [Required]
        public DateTime BugCreationTimeStamp { get; set; }

        public DateTime BugCompletedTimeStamp { get; set; }
    }
}
