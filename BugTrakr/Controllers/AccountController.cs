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
    public class AccountController : Controller
    {
        private UserManager<AppUser> UserMgr { get; }
        private SignInManager<AppUser> SignInMgr { get; }

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            UserMgr = userManager;
            SignInMgr = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(MergeModel user)
        {
            
            var result = await SignInMgr.PasswordSignInAsync(user.AppUsers.UserName, user.AppUserPassword, false, false);
            if (result.Succeeded)
            {
               return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public async Task<IActionResult> Logout() 
        {
            await SignInMgr.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(MergeModel user)
        {
            try
            {
                ViewBag.Message = "User already registered.";
                AppUser appuser = await UserMgr.FindByNameAsync(user.AppUsers.UserName);
                if (appuser == null)
                {
                    appuser = new AppUser();
                    appuser.UserName = user.AppUserName;
                    appuser.Email = user.AppUserEmail;
                    appuser.FirstName = user.AppUsers.FirstName;
                    appuser.LastName = user.AppUsers.LastName;

                    IdentityResult result = await UserMgr.CreateAsync(appuser, user.AppUserPassword);
                    
                    ViewBag.Message = "User was successfully created!";
                    
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {

                ViewBag.Message = ex.Message;
            }

            return View();
        }

        //public IActionResult Delete() 
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Delete(AppUser appuser)
        //{
        //    AppUser user = await UserMgr.FindByNameAsync(appuser.UserName);
        //    if (user != null)
        //    {
        //        await UserMgr.DeleteAsync(user);
        //    }
        //    return View();
        //}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
