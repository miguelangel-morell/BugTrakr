using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BugTrakr.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BugTrakr.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> UserMgr;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager)
        {
            _logger = logger;
            UserMgr = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var fullName = "";
            if (User.Identity.IsAuthenticated)
            {
                var user = await UserMgr.GetUserAsync(HttpContext.User);
                fullName = $"{user.FirstName} {user.LastName}";
            }

            ViewData["FullName"] = fullName;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("/Home")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
