using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrakr.Models
{
    public class UserRoleModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsSelectedUser { get; set; }

        public string FullName 
        {
            get { return FirstName + " " + LastName; }
        }
    }
}
