using BilgeIK.CORE.Entity.Enums;
using BilgeIK.CORE.Service;
using BilgeIK.MODEL.Entities;
using BilgeIK.SERVICE.Helper;
using BilgeIK.WEBUI.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagedList.Core;

namespace BilgeIK.WEBUI.Areas.Administrator.Controllers
{

    public class MembersController : BaseController
    {
        private readonly ICoreService<Definition> defContext;
        private readonly ICoreService<Company> comContext;
        private readonly ICoreService<Order> comOrder;
        private readonly ICoreService<CompanyPackage> comPacks;
        private readonly ICoreService<UserDocument> userDoc;
        private readonly ICoreService<Leave> userLeave;
        private readonly ICoreService<Report> userReport;
        private readonly ICoreService<SalaryBonus> userSalaryBonus;
        private readonly ICoreService<CustomerComment> commentContext;
        private IHostingEnvironment _hostingEnv;
        public MembersController(ILogger<DashboardController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, ICoreService<Definition> defContext, ICoreService<Company> comContext, ICoreService<Order> comOrder, IHostingEnvironment hostingEnv, ICoreService<CompanyPackage> comPacks, ICoreService<UserDocument> userDoc, ICoreService<Leave> userLeave, ICoreService<Report> userReport, ICoreService<SalaryBonus> userSalaryBonus, ICoreService<CustomerComment> commentContext) : base(logger, userManager, signInManager, roleManager)
        {
            this.defContext = defContext;
            this.comContext = comContext;
            this.comOrder = comOrder;
            _hostingEnv = hostingEnv;
            this.comPacks = comPacks;
            this.userDoc = userDoc;
            this.userLeave = userLeave;
            this.userReport = userReport;
            this.userSalaryBonus = userSalaryBonus;
            this.commentContext = commentContext;
        }
        [Breadcrumb(Title = "Üyeler", AreaName = "Administrator", FromController =typeof(DashboardController))]
        public async Task<IActionResult> MemberList(int page = 1, int pageSize = 3)
        {
            var list = new List<UserListVM>();
           
            foreach (AppUser user in userManager.Users.OrderBy(x=>x.UserName).ToList())
            {
                if (user.CompanyId==null)
                {
                    list.Add(new UserListVM()
                    {
                        Id = user.Id,
                        UserEmail = user.Email,
                        UserName = user.UserName,
                        CompanyName="---",
                        Status = user.Status,
                        Roles = await userManager.GetRolesAsync(user)
                    });
                }
                else
                {
                    Guid yeni = user.CompanyId ?? new Guid();
                    list.Add(new UserListVM()
                    {                  
                        Id=user.Id,
                        UserEmail = user.Email,
                        UserName = user.UserName,
                        Status = user.Status,
                        CompanyName = comContext.GetByID(yeni).CompanyName.ToString()??"---",
                        Roles = await userManager.GetRolesAsync(user)
                    }); 
                }

            }

            return View(list.AsQueryable().ToPagedList(page,pageSize));
        }


        [Breadcrumb(Title ="Üye Düzenleme", AreaName ="Administrator", FromAction ="MemberList")]
        public async Task<IActionResult> EditMember(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user==null)
            {
                return RedirectToAction("MemberList");
            }

            UserVM uservm = user.Adapt<UserVM>();

            return View(uservm);
        }

        [HttpPost]
        public async Task<IActionResult> EditMember(UserVM userVM, List<IFormFile> Photo)
        {

            AppUser user = await userManager.FindByIdAsync(userVM.Id);
            user.NationalIDCardNumber = userVM.NationalIDCardNumber;
            //Kullanıcı adı kontrolü başlangıcı
            if (user.UserName!=userVM.UserName)
            {
                if (await userManager.FindByNameAsync(userVM.UserName)!=null)
                {
                    ModelState.AddModelError("","Bu kullanıcı adı kullanılmaktadır.");
                    return View(userVM);
                }
                else
                {
                    user.UserName = userVM.UserName;
                }
            }
            //Kullanıcı adı kontrolü bitişi
            user.FirstName = userVM.FirstName;
            user.LastName = userVM.LastName;
            user.BirthDate = userVM.BirthDate;
            user.Adress = userVM.Adress;
            user.Gender = userVM.Gender;
            user.Status = userVM.Status;
            user.LockoutEnd = userVM.LockoutEnd;
            user.JobTitle = userVM.JobTitle;
            //Telefon numarası kontrolü başlangıcı           
            if (user.PhoneNumber!=userVM.PhoneNumber)
            {
                if (userManager.Users.Any(u => u.PhoneNumber == userVM.PhoneNumber))
                {
                    ModelState.AddModelError("", "Bu telefon numarasını başka bir üye kullanmaktadır.");
                    return View(userVM);
                }
                else
                {
                    user.PhoneNumber = userVM.PhoneNumber;
                }
            }
            //Telefon numarası kontrolü bitişi
            //Email adresi kontrolü başlangıcı
            if (user.Email!=userVM.Email)
            {
                if (userManager.Users.Any(u => u.Email == userVM.Email))
                {
                    ModelState.AddModelError("", "Bu e-posta adresini başka bir üye kullanmaktadır.");
                    return View(userVM);
                }
                else
                {
                    user.Email = userVM.Email;
                }
            }

            //Email adresi kontrolü bitişi
            user.EmailConfirmed = userVM.EmailConfirmed;
            user.LockoutEnabled = userVM.LockoutEnabled;


            string imgPath = Upload.ImageUpload(Photo, _hostingEnv, out bool imgResult);
            if (imgResult)
            {
                user.Photo = imgPath;
            }

            if (ModelState.IsValid)
            {
                await userManager.UpdateAsync(user);
                TempData["returnMessage"] = "success";
                return RedirectToAction("EditMember", "Members", new { Id = user.Id });
            }

            return View(userVM);
        }

        [Breadcrumb(Title = "Üye Profili", AreaName = "Administrator", FromAction = "MemberList")]
        public async Task<IActionResult> DetailsMember(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user == null) { 
                return RedirectToAction("MemberList","Members");
            }

            IList<string> role = await userManager.GetRolesAsync(user);
            string CompanyName = "---";
            if (user.CompanyId != null)
            {
                Guid yeni = user.CompanyId ?? new Guid();
                CompanyName = comContext.GetByID(yeni).CompanyName.ToString() ?? "---";
            }

            List<UserDocument> documents = userDoc.GetDefault(x => x.UserId == Guid.Parse(user.Id)).TakeLast(10).OrderByDescending(x => x.CreatedDate).ToList();

            List<Leave> leaves = userLeave.GetDefault(x => x.UserId == Guid.Parse(user.Id)).TakeLast(10).ToList();

            List<Definition> definitions = defContext.GetAll();
            //InnerJoin LinQ Başlangıç
            leaves = leaves.Join(definitions, Leave => Leave.DefinitionId, Definition => Definition.Id, (Leave, Definition) => Leave).ToList();
            //InnerJoin LinQ Bitiş


            List<Report> reports = userReport.GetDefault(x => x.UserId == Guid.Parse(user.Id)).TakeLast(10).ToList();
            //InnerJoin LinQ Başlangıç
            reports = reports.Join(definitions, Report => Report.DefinitionId, Definition => Definition.Id, (Report, Definition) => Report).ToList();
            //InnerJoin LinQ Bitiş
            
            
            List<SalaryBonus> salaryBonus = userSalaryBonus.GetDefault(x => x.UserId == Guid.Parse(user.Id)).TakeLast(10).ToList();

            return View(Tuple.Create<UserVM, IList<string>, string, List<UserDocument>, List<Leave>, List<Report>, List<SalaryBonus>> (user.Adapt<UserVM>(), role, CompanyName, documents, leaves, reports, salaryBonus));
        }

        [HttpPost]
        public IActionResult MemberDocumentSender(string Name, string UserId, List<IFormFile> Files)
        {

            if (userManager.Users.Any(u=>u.Id==UserId))
            {
                string filePath = Upload.FileUpload(Files, _hostingEnv, out bool fileResult);
                if (fileResult)
                {
                    UserDocument userDocument = new UserDocument()
                    {
                        Name = Name,
                        FileUrl = filePath,
                        UserId = Guid.Parse(UserId),
                        Status = CORE.Entity.Enums.Status.Actived,
                        CreatedDate = DateTime.Now
                    };

                    if (userDoc.Add(userDocument))
                    {
                        TempData["returnMessage"] = "success";
                    }
                    else
                    {
                        TempData["returnMessage"] = "error";
                    }
                }
                else
                {
                    TempData["returnMessage"] = "error";
                }
                return RedirectToAction("DetailsMember", "Members", new { Id = UserId });
            }
            else
            {
                return RedirectToAction("MemberList", "Members");
            }            
        }
        [Breadcrumb(Title = "Üye Dosyaları", AreaName = "Administrator", FromAction = "MemberList")]
        public IActionResult EditMemberDocument(Guid id)
        {
            UserDocument document= userDoc.GetByID(id);

            return View(document.Adapt<UserDocVM>());
        }

        [HttpPost]
        public IActionResult EditMemberDocument(UserDocVM userDocVM)
        {
            UserDocument document = userDoc.GetByID(Guid.Parse(userDocVM.Id));

            if (document==null)
            {
                return RedirectToAction("MemberList");
            }

            if (ModelState.IsValid)
            {
                document.Name = userDocVM.Name;
                document.Status = userDocVM.Status;
                if (userDoc.Update(document))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("DetailsMember","Members", new { Id=document.UserId});
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    return View(userDocVM);
                }
            }

            return View(userDocVM);

        }

        public IActionResult RemoveMemberDocument(Guid id)
        {
            UserDocument document = userDoc.GetByID(id);
            if (document==null)
            {
                return RedirectToAction("MemberList","Members");
            }

            if (userDoc.Remove(document))
            {
                TempData["returnMessage"] = "success";
            }
            else
            {
                TempData["returnMessage"] = "error";
            }
            return RedirectToAction("DetailsMember", "Members", new { Id = document.UserId });
        }


        [Breadcrumb(Title = "Üye Dosyaları", AreaName = "Administrator", FromAction = "MemberList")]
        public async Task<IActionResult> MemberDocuments(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("MemberList", "Members");
            }

            IList<string> role = await userManager.GetRolesAsync(user);
            string CompanyName = "---";
            if (user.CompanyId != null)
            {
                Guid yeni = user.CompanyId ?? new Guid();
                CompanyName = comContext.GetByID(yeni).CompanyName.ToString() ?? "---";
            }

            List<UserDocument> documents = userDoc.GetDefault(x => x.UserId == Guid.Parse(user.Id)).OrderByDescending(x=>x.CreatedDate).ToList();


            return View(Tuple.Create<UserVM, IList<string>, string, List<UserDocument>>(user.Adapt<UserVM>(), role, CompanyName, documents));

        }
          
        public async Task<IActionResult> DetailsMemberLeave(Guid id)
        {
            LeaveVM leave = userLeave.GetByID(id).Adapt<LeaveVM>();
            leave.AppUser = await userManager.FindByIdAsync(leave.UserId.ToString());
            ViewBag.IzinTuru = defContext.GetByID(leave.DefinitionId).Name.ToString();
            return View(leave);
        }

        public IActionResult EditMemberLeave(Guid id)
        {
            Leave leave = userLeave.GetByID(id);
            if (leave == null)
            {
                return NotFound();
            }
            LeaveVM leaveVM = leave.Adapt<LeaveVM>();
            leaveVM.Definition = defContext.GetDefault(x => x.DefinitionType == CORE.Entity.Enums.DefinitionType.Leave);
            return View(leaveVM);
        }

        [HttpPost]
        public IActionResult EditMemberLeave(LeaveVM leaveVM)
        {
            Leave leave = leaveVM.Adapt<Leave>();

            if (ModelState.IsValid)
            {
                if (userLeave.Update(leave))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("DetailsMember", "Members", new { Id = leave.UserId });
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    ModelState.AddModelError("", "İzin güncelleme esnasında hata ile karşılaşıldı.");
                }

            }
            return View(leaveVM);
        }

        public IActionResult RemoveMembersLeave(Guid id)
        {
            Leave leave = userLeave.GetByID(id);
            if (leave == null)
            {
                return NotFound();
            }

            leave.Status = CORE.Entity.Enums.Status.Deleted;
            
            if (userLeave.Update(leave))
            {
                TempData["returnMessage"] = "success";
            }
            else
            {
                TempData["returnMessage"] = "error";
            }

            return RedirectToAction("DetailsMember", "Members", new { Id = leave.UserId });

        }


        public async Task<IActionResult> DetailsMemberReport(Guid id)
        {
            Report report = userReport.GetByID(id);
            if (report == null)
            {
                return NotFound();
            }

            report.AppUser = await userManager.FindByIdAsync(report.UserId.ToString());
            ViewBag.RaporTuru = defContext.GetByID(report.DefinitionId).Name.ToString();

            return View(report.Adapt<ReportVM>());
        }

        public IActionResult EditMemberReport(Guid id)
        {
            Report report = userReport.GetByID(id);
            if (report == null)
            {
                return NotFound();
            }
            ReportVM reportVM = report.Adapt<ReportVM>();
            reportVM.Definition = defContext.GetDefault(x => x.DefinitionType == CORE.Entity.Enums.DefinitionType.Report);
            return View(reportVM);
        }

        [HttpPost]
        public IActionResult EditMemberReport(ReportVM reportVM)
        {
            Report report = reportVM.Adapt<Report>();

            if (ModelState.IsValid)
            {
                if (userReport.Update(report))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("DetailsMember", "Members", new { Id = report.UserId });
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    ModelState.AddModelError("", "Rapor güncelleme esnasında hata ile karşılaşıldı.");
                }

            }
            return View(reportVM);
        }

        public IActionResult RemoveMemberReport(Guid id)
        {
            Report report = userReport.GetByID(id);
            if (report == null)
            {
                return NotFound();
            }

            report.Status = CORE.Entity.Enums.Status.Deleted;

            if (userReport.Update(report))
            {
                TempData["returnMessage"] = "success";
            }
            else
            {
                TempData["returnMessage"] = "error";
            }

            return RedirectToAction("DetailsMember", "Members", new { Id = report.UserId });

        }


        public async Task<IActionResult> DetailsMemberSBonus(Guid id)
        {
            SalaryBonusVM salaryBonusVM = userSalaryBonus.GetByID(id).Adapt<SalaryBonusVM>();
            salaryBonusVM.AppUser = await userManager.FindByIdAsync(salaryBonusVM.UserId.ToString());
            salaryBonusVM.ApproverUser = await userManager.FindByIdAsync(salaryBonusVM.ApproverUserId.ToString());

            return View(salaryBonusVM);
        }

        public IActionResult EditMemberSBonus(Guid id)
        {
            SalaryBonusVM salaryBonusVM = userSalaryBonus.GetByID(id).Adapt<SalaryBonusVM>();
            if (salaryBonusVM == null)
            {
                return NotFound();
            }

            return View(salaryBonusVM);
        }

        [HttpPost]
        public IActionResult EditMemberSBonus(SalaryBonusVM salaryBonusVM)
        {
            if (ModelState.IsValid)
            {
                
                if (userSalaryBonus.Update(salaryBonusVM.Adapt<SalaryBonus>()))
                {
                    TempData["returnMessage"] = "success";
                }
                else
                {
                    TempData["returnMessage"] = "error";
                }
                return RedirectToAction("DetailsMember", "Members", new { Id = salaryBonusVM.UserId });
            }

            return View(salaryBonusVM);
        }

        public IActionResult RemoveMemberSBonus(Guid id)
        {
            SalaryBonus salaryBonus = userSalaryBonus.GetByID(id);
            if (salaryBonus == null)
            {
                return NotFound();
            }

            salaryBonus.Status = CORE.Entity.Enums.Status.Deleted;
            userSalaryBonus.Update(salaryBonus);

            return RedirectToAction("DetailsMember", "Members", new { Id = salaryBonus.UserId });
        }

        [Breadcrumb(AreaName ="Administrator", Title ="Kullanıcı Yorumları", FromController = typeof(DashboardController))]
        public IActionResult CustomerComments(int page = 1, int pageSize = 3)
        {


            var comments = from comment in commentContext.GetAll()
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

        [Breadcrumb(Title = "Yorum Düzenleme Formu", AreaName = "Administrator", FromAction = "CustomerComments")]
        public IActionResult EditComment(Guid id)
        {
            CustomerComment comment = commentContext.GetByID(id);

            if (comment==null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [HttpPost]
        public IActionResult EditComment(CustomerComment customerComment)
        {
            CustomerComment comment = commentContext.GetByID(customerComment.Id);

            if (comment==null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                comment.Comment = customerComment.Comment;
                comment.Status = customerComment.Status;
                comment.FeaturedComment = customerComment.FeaturedComment;

                if (commentContext.Update(comment))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("CustomerComments");
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    ModelState.AddModelError("","Güncelleme esnasında hata ile karşılaşıldı.");
                }

            }

            return View(customerComment);
        }

    }
}
