using BilgeIK.CORE.Service;
using BilgeIK.MODEL.Context;
using BilgeIK.MODEL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.Areas.Manager.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly ILogger<ManagerDashboardController> _logger;
        protected UserManager<AppUser> userManager { get; }
        protected SignInManager<AppUser> signInManager { get; }
        protected RoleManager<AppRole> roleManager { get; }
        protected AppUser CurrentUser => userManager.FindByNameAsync(User.Identity.Name).Result;
        public BaseController(ILogger<ManagerDashboardController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager = null)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }


        public void AddModelError(IdentityResult result)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
        }

    }
}
