using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrakr.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        [StringLength(int.MaxValue)]
        [Display(Name ="Comments")]
        public string CommentText { get; set; }

        [ForeignKey("Bug")]
        public int BugId { get; set; }

        public int UserId { get; set; }
    }
}
