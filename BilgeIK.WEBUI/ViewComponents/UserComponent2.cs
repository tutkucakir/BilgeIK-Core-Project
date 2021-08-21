using BilgeIK.MODEL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewComponents
{
    public class UserComponent2 : ViewComponent
    {
        private UserManager<AppUser> _userManager; 
        public UserComponent2(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(user);
        }
    }
}
