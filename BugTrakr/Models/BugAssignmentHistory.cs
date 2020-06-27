using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrakr.Models
{
    public class BugAssignmentHistory
    {
        public int BugAssignmentHistoryId { get; set; }

        [ForeignKey("Bug")]
        public int BugId { get; set; }

        public BugStatusEnum BugStatus { get; set; }
        
        public int AssignedToId { get; set; }

        public string RoleName { get; set; }
    }
}
