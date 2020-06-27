using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrakr.Models
{

    public enum BugStatusEnum
    {
        [Description("Awaiting Approval")]
        [Display(Name = "Awaiting Approval")]
        AwaitingApproval,
        [Display(Name = "In Progress")]
        InProgress,
        Done,
        Completed
    }

    public enum BugDifficultyEnum
    {
        Easy,
        Meduim,
        Hard
    }

    public enum NotificationTypeEnum 
    {
        BugAssigned,
        RequestBugReview,
        BugCompleted,
        BugRemoved,
        BugDeleted,
        NewBugComment,
        BugUpdated
    }
}
