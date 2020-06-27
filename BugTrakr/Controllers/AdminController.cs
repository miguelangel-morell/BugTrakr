using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTrakr.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BugTrakr.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<AppRole> RoleMgr;

        private readonly UserManager<AppUser> UserMgr;

        public AdminController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            RoleMgr = roleManager;
            UserMgr = userManager;
        }

        
        public IActionResult Index()
        {
            var role = RoleMgr.Roles.ToList();
            return View(role);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var result = await RoleMgr.FindByIdAsync(id);
            
            if (result != null)
            {
                var model = new MergeModel
                {
                    RoleId = result.Id,
                    RoleName = result.Name
                };

                //Collect all users that are associated with the Role Name 
                foreach (var user in UserMgr.Users) 
                {
                    if (await UserMgr.IsInRoleAsync(user, result.Name))
                    {
                        model.UserNames.Add(user.UserName);
                        model.UserFullNames.Add(user.FirstName + " " + user.LastName);
                    }
                }

                return View(model);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(MergeModel mergeModel)
        {
            var role = await RoleMgr.FindByIdAsync(mergeModel.RoleId.ToString());

            if (role != null)
            {
                role.Name = mergeModel.RoleName;
                var result = await RoleMgr.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }
            }

            return View(mergeModel);
        }


        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(MergeModel modelRole)
        {
            AppRole role = new AppRole
            {
                Name = modelRole.RoleName
            };

            IdentityResult result = await RoleMgr.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Admin");
            }


            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditUserRoles(int roleId)
        {
            ViewBag.roleId = roleId;
            var role = await RoleMgr.FindByIdAsync(roleId.ToString());
            
            var model = new List<UserRoleModel>();
            
            if (role != null)
            {
                

                foreach (var user in UserMgr.Users)
                {
                    var userRoleModel = new UserRoleModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    };

                    if (await UserMgr.IsInRoleAsync(user, role.Name))
                    {
                        userRoleModel.IsSelectedUser = true;
                    }
                    else
                    {
                        userRoleModel.IsSelectedUser = false;
                    }

                    model.Add(userRoleModel);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserRoles(List<UserRoleModel> model, string roleId)
        {
            var role = await RoleMgr.FindByIdAsync(roleId);

            if (role != null)
            {
                for (int i = 0; i < model.Count; i++)
                {
                    var user = await UserMgr.FindByIdAsync(model[i].UserId.ToString());
                    
                    IdentityResult result = null;

                    if (model[i].IsSelectedUser && !(await UserMgr.IsInRoleAsync(user, role.Name)))
                    {
                        result = await UserMgr.AddToRoleAsync(user, role.Name);
                    }
                    else if (!model[i].IsSelectedUser && await UserMgr.IsInRoleAsync(user, role.Name))
                    {
                        result = await UserMgr.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue;
                    }

                    if (result.Succeeded)
                    {
                        if (i < model.Count - 1)
                        {
                            continue;
                        }
                        else
                        {
                            return RedirectToAction("EditRole", new { id = roleId });
                        }
                    }
                }
            }
            return RedirectToAction("EditRole", new { id = roleId });
        }
    }
}
