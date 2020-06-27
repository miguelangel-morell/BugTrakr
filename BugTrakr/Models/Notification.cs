using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BugTrakr.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public NotificationTypeEnum NotificationType { get; set; }

        [StringLength(500)]
        public string NotificationMessage { get; set; }

        public int ReceipientId { get; set; }

        public int SenderId { get; set; }

        [ForeignKey("Bug")]
        public int BugId { get; set; }
    }
}
