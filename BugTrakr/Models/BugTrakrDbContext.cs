using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrakr.Models
{
    public class BugTrakrDbContext  :   DbContext
    {
        public BugTrakrDbContext(DbContextOptions<BugTrakrDbContext> options):base(options)
        {
        }

        public DbSet<Bug> Bugs { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<BugAssignmentHistory> BugAssignmentHistories { get; set; }

        public DbSet<Notification> Notifications { get; set; }
    }
}
