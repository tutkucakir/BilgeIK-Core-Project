using BilgeIK.CORE.Service;
using BilgeIK.MODEL.Entities;
using BilgeIK.WEBUI.Models;
using BilgeIK.WEBUI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PagedList.Core;

namespace BilgeIK.WEBUI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ICoreService<CustomerComment> commentContext;
        private readonly ICoreService<Company> companyContext;
        private readonly ICoreService<Company> comContext;
        private readonly ICoreService<Contact> contactContext;
        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, ICoreService<CustomerComment> commentContext, ICoreService<Company> companyContext, ICoreService<Company> comContext, ICoreService<Contact> contactContext) : base(logger, userManager, signInManager, roleManager)
        {
            this.commentContext = commentContext;
            this.companyContext = companyContext;
            this.comContext = comContext;
            this.contactContext = contactContext;

        }

        [DefaultBreadcrumb("Portal")]
        public async Task<IActionResult> Index()
        {

            var comments = from comment in commentContext.GetDefault(x => x.FeaturedComment == true && x.Status == CORE.Entity.Enums.Status.Actived).OrderByDescending(x => x.CreatedDate).Take(4)
                           join user in userManager.Users.ToList() on comment.UserId.ToString() equals user.Id
                           join company in companyContext.GetAll() on user.CompanyId equals company.Id
                           select new
                           {
                               comment,
                               user,
                               company
                           };

            List<CommentsJoinVM> commentlist = new List<CommentsJoinVM>();

            foreach (var item in comments)
            {
                commentlist.Add(new CommentsJoinVM
                {
                    Id = item.comment.Id,
                    CommentContent = item.comment.Comment,
                    FeaturedComment = item.comment.FeaturedComment,
                    AppUser = item.user,
                    Company = item.company,
                    Status = item.comment.Status,
                    CreatedDate = item.comment.CreatedDate
                });
            }

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
                if (userManager.IsInRoleAsync(user, "Site Yöneticisi").Result)
                {
                    ViewBag.yetki = "Admin";
                }
                else if (userManager.IsInRoleAsync(user, "Şirket Temsilcisi").Result)
                {
                    ViewBag.yetki = "Manager";
                }
                else
                {
                    ViewBag.yetki = "Personal";
                }
            }


            return View(commentlist);
        }

        public IActionResult Comments(int page = 1, int pageSize = 3)
        {
            var comments = from comment in commentContext.GetActive()
                           join user in userManager.Users.ToList() on comment.UserId.ToString() equals user.Id
                           join company in comContext.GetAll() on user.CompanyId equals company.Id
                           select new
                           {
                               comment,
                               user,
                               company
                           };

            List<CommentsJoinVM> commentlist = new List<CommentsJoinVM>();

            foreach (var item in comments)
            {
                commentlist.Add(new CommentsJoinVM
                {
                    Id = item.comment.Id,
                    CommentContent = item.comment.Comment,
                    FeaturedComment = item.comment.FeaturedComment,
                    AppUser = item.user,
                    Company = item.company,
                    Status = item.comment.Status,
                    CreatedDate = item.comment.CreatedDate
                });
            }


            return View(commentlist.AsQueryable().ToPagedList(page,pageSize));
        }

        [HttpPost]
        public IActionResult Contact(string name, string email, string message)
        {
            Contact contact = new() {
                Id = Guid.NewGuid(),
                Name = name,
                Mail = email,
                Message = message,
                PostedDate = DateTime.Now,
                Status = CORE.Entity.Enums.Status.Pending            
            };

            if (contactContext.Add(contact))
            {
                ViewBag.message = "success";
            }
            else
            {
                ViewBag.message = "error";
            }


            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
