using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTrakr.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BugTrakr.Controllers
{
    public class TeamController : Controller
    {
        private readonly BugTrakrDbContext _context;
        private readonly UserManager<AppUser> UserMgr;

        public TeamController(BugTrakrDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            UserMgr = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = UserMgr.Users.ToList();

            MergeModel model = new MergeModel();

            List<BugAssignmentHistory> bugHistoryList =
                new List<BugAssignmentHistory>();

            foreach (var user in users)
            {
                if (await UserMgr.IsInRoleAsync(user, "Developer"))
                {
                    var bugHistory = _context
                                    .BugAssignmentHistories
                                    .Where(bh => bh.AssignedToId == user.Id);
                    
                    bugHistoryList.AddRange(bugHistory);
                }
            }

            //gray
            model.ApprovalStatusCount = GetTeamProgressData(bugHistoryList, BugStatusEnum.AwaitingApproval);
            
            //blue
            model.InProgressStatusCount = GetTeamProgressData(bugHistoryList, BugStatusEnum.InProgress);
            
            //green
            model.DoneStatusCount = GetTeamProgressData(bugHistoryList, BugStatusEnum.Done);
            
            //purple
            model.CompletedStatusCount = GetTeamProgressData(bugHistoryList,BugStatusEnum.Completed);

            return View(model);
        }

        /// <summary>
        /// Use this to return data for each status in Bug Assignment History list.
        /// </summary>
        /// <param name="bugHistory"></param>
        /// <param name="bugStatus"></param>
        /// <returns></returns>
        private int GetTeamProgressData(List<BugAssignmentHistory> bugHistory,
            BugStatusEnum bugStatus)
        {
            int progressData = bugHistory
                               .Where(bH => bH.BugStatus == bugStatus).ToList().Count;

            return progressData;
        }
    }
}
