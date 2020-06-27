using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTrakr.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BugTrakr.Controllers
{
    public class CommentController : Controller
    {
        private readonly UserManager<AppUser> UserMgr;
        private readonly BugTrakrDbContext _context;

        public CommentController(UserManager<AppUser> userManager, BugTrakrDbContext context)
        {
            UserMgr = userManager;
            _context = context;
        }

        /// <summary>
        /// This action method takes in the MergeModel object and assigns a new comment that
        /// will be tied to the bug it was added on.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddComment(MergeModel model)
        {
            Bug bug = model.Bugs;

            //access logged in user's User record and Role
            var userLoggedInId = Convert.ToInt32(UserMgr.GetUserId(HttpContext.User));
            var currentUser = await UserMgr.FindByIdAsync(userLoggedInId.ToString());
            var currentUserRole = await UserMgr.GetRolesAsync(currentUser);

            var userAssignedTo = await UserMgr.FindByIdAsync(bug.AssignedToId.ToString());
            
            
            /*If the current user is adding a comment to their bug then no need
             * send out a notification.
             */
            if (userLoggedInId != bug.AssignedToId)
            {
                var notification = new Notification
                {
                    NotificationType = NotificationTypeEnum.NewBugComment,
                    NotificationMessage = $"Hey {userAssignedTo.FirstName}, " +
                                            $"({currentUserRole[0]}) {currentUser.UserName}" +
                                            $" just added a new comment to Bug #{bug.BugId}",
                    ReceipientId = bug.AssignedToId,
                    SenderId = bug.AssignedById,
                    BugId = bug.BugId
                };

                _context.Add(notification);
            }


            var comments = new Comment
            {
                CommentText = model.Comment.CommentText,
                BugId = bug.BugId,
                UserId = userLoggedInId
            };

            _context.Add(comments);
            
            
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Bug", new { id = bug.BugId });
        }
    }
}
