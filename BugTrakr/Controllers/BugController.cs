using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugTrakr.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Policy;
using Microsoft.AspNetCore.Authorization;

namespace BugTrakr.Controllers
{
    public class BugController : Controller
    {
        private readonly BugTrakrDbContext _context;
        private readonly UserManager<AppUser> UserMgr;
        private readonly RoleManager<AppRole> RoleMgr;

        public BugController(UserManager<AppUser> userManager,
            BugTrakrDbContext context, RoleManager<AppRole> roleManager)
        {
            UserMgr = userManager;
            RoleMgr = roleManager;
            _context = context;
        }

        // GET: Bug
        [Authorize(Roles = "QA, Tech Lead, Developer")]
        public async Task<IActionResult> Index()
        {
            //Developers can only view bugs assigned to them
            var currentUser = await UserMgr.GetUserAsync(HttpContext.User);
            
            List<Bug> devBugs = _context.Bugs
                                    .Where(b => b.AssignedToId == currentUser.Id).ToList();

            if (await UserMgr.IsInRoleAsync(currentUser, "Developer"))
            {
                return View(devBugs);
            }
            return View(await _context.Bugs.ToListAsync());
        }

        // GET: Bug/Details/5
        [Authorize(Roles="QA, Tech Lead, Developer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Home");
            }

            MergeModel model = new MergeModel();
            
            var bug = await _context.Bugs
                .FirstOrDefaultAsync(m => m.BugId == id);


            if (bug == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var comments = _context.Comments.Where(m => m.BugId == id).ToList();

            model.Bugs = bug;
            model.Comments = comments;
            model.Users.Add(await UserMgr.FindByIdAsync(bug.AssignedToId.ToString()));
            model.Users.Add(await UserMgr.FindByIdAsync(bug.AssignedById.ToString()));
            model.AppUsers = await UserMgr.GetUserAsync(HttpContext.User);

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles ="QA")]
        // GET: Bug/Create
        public IActionResult Create()
        {
            MergeModel model = new MergeModel();
            model.Users = UserMgr.Users.ToList();
            return View(model);
        }

        // POST: Bug/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles ="QA")]
        public async Task<IActionResult> Create(MergeModel model)
        {

            var assignedBy = UserMgr.GetUserId(HttpContext.User);

            Bug bug = model.Bugs;
            bug.BugCreationTimeStamp = DateTime.Now;
            bug.AssignedById = Convert.ToInt32(assignedBy);
            
            _context.Add(bug);
            await _context.SaveChangesAsync();

            var user = UserMgr.Users.FirstOrDefault(m => m.Id == bug.AssignedToId);
            var userAssignedBy = UserMgr.Users.FirstOrDefault(m => m.Id == bug.AssignedById);
            var role = await UserMgr.GetRolesAsync(user);
            var roleAssignedBy = await UserMgr.GetRolesAsync(userAssignedBy);

            //check if the user assigned to the bug is already in the bug history model
            var userAssigned = _context.BugAssignmentHistories.FirstOrDefault(m => 
                    m.AssignedToId == bug.AssignedToId && m.BugId == bug.BugId);

            var notification = new Notification
            {
                NotificationType    = NotificationTypeEnum.BugAssigned,
                NotificationMessage = $"Hey {user.FirstName}, you've" +
                                      " been assigned a new Bug by " +
                                      $"({roleAssignedBy[0]}) {userAssignedBy.UserName}!",
                ReceipientId        = bug.AssignedToId,
                SenderId            = bug.AssignedById,
                BugId               = bug.BugId
            };

            _context.Add(notification);

            var bugHistory = new BugAssignmentHistory
            {
                BugId = bug.BugId,
                BugStatus = bug.BugStatus,
                AssignedToId = bug.AssignedToId,
                RoleName = role[0]
            };

            if (userAssigned == null)
            {
                _context.Add(bugHistory);
            }
            else
            {
                _context.Update(bugHistory);
            }


            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Bug");

        }

        // GET: Bug/Edit/5
        [Authorize(Roles = "QA, Tech Lead, Developer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Bug bug = await _context.Bugs.FindAsync(id);

            if (bug == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(bug);
        }

        // POST: Bug/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Bug bug)
        {
            
            var userAssignedTo = await UserMgr.FindByIdAsync(bug.AssignedToId.ToString());
            
            var currentUserId = UserMgr.GetUserId(HttpContext.User);
            
            var currentUser = await UserMgr.FindByIdAsync(currentUserId);
            
            var currentUserRole = await UserMgr.GetRolesAsync(currentUser);
            
            var roleAssignedTo = await UserMgr.GetRolesAsync(userAssignedTo);

            if (id != bug.BugId)
            {
                return RedirectToAction("Error", "Home");
            }

            /* 
             * Depending on who the bug is assigned to, set the current user
             * to be the one assigned by the bug.
             */
            if (await UserMgr.IsInRoleAsync(userAssignedTo, "Tech Lead"))
            {
                bug.AssignedById = Convert.ToInt32(currentUserId);
            }
            else if (await UserMgr.IsInRoleAsync(userAssignedTo, "Developer") &&
                    User.IsInRole("Tech Lead"))
            {
                bug.AssignedById = Convert.ToInt32(currentUserId);
            }
            else if (await UserMgr.IsInRoleAsync(userAssignedTo, "QA") &&
                   User.IsInRole("Tech Lead"))
            {
                bug.AssignedById = Convert.ToInt32(currentUserId);
            }

            /*
             * Don't send a notification if the current user is assigned 
             * to the bug they are editing.
            */
            if (currentUserId != bug.AssignedToId.ToString())
            {
                var notification = new Notification
                {
                    NotificationType = NotificationTypeEnum.BugUpdated,
                    NotificationMessage = $"Hey {userAssignedTo.FirstName}, Bug" +
                                          $" #{bug.BugId} was just updated by " +
                                          $"({currentUserRole[0]}) {currentUser.UserName}!",
                    ReceipientId = bug.AssignedToId,
                    SenderId = currentUser.Id,
                    BugId = bug.BugId
                };

                _context.Add(notification);
            }

            /*
            * Check if the user has been added to the history table,
            * if not then create a new object. Otherwise just update the status.
            */
            var bugAssignedHistory = _context.BugAssignmentHistories.FirstOrDefault(m =>
                            m.AssignedToId == bug.AssignedToId && m.BugId == bug.BugId);

            if (bugAssignedHistory == null)
            {
                var bugHistory = new BugAssignmentHistory
                {
                    BugId           = bug.BugId,
                    BugStatus       = bug.BugStatus,
                    AssignedToId    = bug.AssignedToId,
                    RoleName        = roleAssignedTo[0]
                };

                _context.Add(bugHistory);
            }
            else
            {
                bugAssignedHistory.BugStatus = bug.BugStatus;
                
                _context.Update(bugAssignedHistory);
            }

            _context.Bugs.Update(bug);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Bug");
        }

        // GET: Bug/Delete/5
        [Authorize(Roles = "QA")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var bug = await _context.Bugs
                .FirstOrDefaultAsync(m => m.BugId == id);

            if (bug == null)
            {
                return RedirectToAction("Error", "Home");
            }

            MergeModel model = new MergeModel();

            model.Bugs = bug;
            model.Users.Add(await UserMgr.FindByIdAsync(bug.AssignedToId.ToString()));
            model.Users.Add(await UserMgr.FindByIdAsync(bug.AssignedById.ToString()));

            return View(model);
        }

        // POST: Bug/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "QA")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bug = await _context.Bugs.FindAsync(id);
            var comments = _context.Comments.Where(c => c.BugId == bug.BugId);
            var notifications = _context.Notifications.Where(n => n.BugId == bug.BugId);
            var bugHistory = _context.BugAssignmentHistories.Where(bH => bH.BugId == bug.BugId);
            
            if (comments != null)
            {
                _context.Comments.RemoveRange(comments);
            }

            _context.BugAssignmentHistories.RemoveRange(bugHistory);
            _context.Notifications.RemoveRange(notifications);
            _context.Bugs.Remove(bug);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Bug");
        }

        public async Task<IActionResult> CompleteBug(int id)
        {
            var bug = await _context.Bugs.FirstOrDefaultAsync(m => m.BugId == id);
            
            bug.BugCompletedTimeStamp = DateTime.Now;
            bug.IsComplete = true;
            bug.BugStatus = BugStatusEnum.Completed;

            var bugHistory = _context.BugAssignmentHistories.Where(bH => bH.BugId == bug.BugId);

            foreach (var bugH in bugHistory)
            {
                bugH.BugStatus = BugStatusEnum.Completed;
            }

            _context.BugAssignmentHistories.UpdateRange(bugHistory);
            _context.Bugs.Update(bug);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Bug");
        }

        private bool BugExists(int id)
        {
            return _context.Bugs.Any(e => e.BugId == id);
        }
    }
}
