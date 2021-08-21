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
namespace BilgeIK.WEBUI.Areas.Manager.Controllers
{

    public class ManagerDashboardController : BaseController
    {
        private readonly ICoreService<Company> comContext;
        private readonly ICoreService<CustomerComment> commentContext;
        private readonly ICoreService<Leave> leaveContext;
        private readonly ICoreService<Report> reportContext;
        private readonly ICoreService<UserDocument> userDocContext;
        private readonly ICoreService<SalaryBonus> salaryBonusContext;
        private readonly ICoreService<Order> orderContext;
        private readonly ICoreService<CompanyPackage> packageContext;
        private IHostingEnvironment _hostingEnv;

        public ManagerDashboardController(ILogger<ManagerDashboardController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, ICoreService<Company> comContext, ICoreService<CustomerComment> commentContext, ICoreService<Leave> leaveContext, ICoreService<Report> reportContext, ICoreService<UserDocument> userDocContext, ICoreService<SalaryBonus> salaryBonusContext, ICoreService<Order> orderContext, ICoreService<CompanyPackage> packageContext, IHostingEnvironment hostingEnv) : base(logger, userManager, signInManager, roleManager)
        {
            this.comContext = comContext;
            this.commentContext = commentContext;
            this.leaveContext = leaveContext;
            this.reportContext = reportContext;
            this.userDocContext = userDocContext;
            this.salaryBonusContext = salaryBonusContext;
            this.orderContext = orderContext;
            this.packageContext = packageContext;
            _hostingEnv = hostingEnv;
        }

        [Breadcrumb("Firma Paneli", AreaName = "Manager")]
        public async Task<IActionResult> Index()
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
            Guid companyId = user.CompanyId ?? new Guid();
            Company company = comContext.GetByID(companyId);


            ViewBag.totalActiveUser = userManager.Users.Count(x => x.CompanyId == companyId && x.Status == CORE.Entity.Enums.Status.Actived).ToString();
            ViewBag.totalPendingUser = userManager.Users.Count(x => x.CompanyId == companyId && x.Status == CORE.Entity.Enums.Status.Pending).ToString();


            List<Leave> leaves = leaveContext.GetDefault(x => x.Status == CORE.Entity.Enums.Status.Actived).ToList();
            List<Leave> leavesPending = leaveContext.GetDefault(x => x.Status == CORE.Entity.Enums.Status.Pending).ToList();

            List<Report> reports = reportContext.GetDefault(x => x.Status == CORE.Entity.Enums.Status.Actived).ToList();
            List<Report> reportsPending = reportContext.GetDefault(x => x.Status == CORE.Entity.Enums.Status.Pending).ToList();

            List<UserDocument> documents = userDocContext.GetDefault(x => x.Status == CORE.Entity.Enums.Status.Actived).ToList();
            List<UserDocument> documentsPending = userDocContext.GetDefault(x => x.Status == CORE.Entity.Enums.Status.Pending).ToList();

            List<AppUser> userList = userManager.Users.Where(x => x.CompanyId == companyId && x.Status == CORE.Entity.Enums.Status.Actived).ToList();


            var leaves2 = from izin in leaves
                          join users in userList on izin.UserId.ToString() equals users.Id
                          select new
                          {
                              izin
                          };

            var leaves2Pending = from izin in leavesPending
                                 join users in userList on izin.UserId.ToString() equals users.Id
                                 select new
                                 {
                                     izin
                                 };

            var reports2 = from rapor in reports
                           join users in userList on rapor.UserId.ToString() equals users.Id
                           select new
                           {
                               rapor
                           };

            var reports2Pending = from rapor in reportsPending
                                  join users in userList on rapor.UserId.ToString() equals users.Id
                                  select new
                                  {
                                      rapor
                                  };


            var documents2 = from document in documents
                             join users in userList on document.UserId.ToString() equals users.Id
                           select new
                           {
                               document
                           };

            var documents2Pending = from document in documentsPending
                                    join users in userList on document.UserId.ToString() equals users.Id
                                  select new
                                  {
                                      document
                                  };

            ViewBag.totalLeaves = leaves2.Count().ToString();

            ViewBag.pendingLeaves = leaves2Pending.Count().ToString();
            ViewBag.totalReports = reports2.Count().ToString();
            ViewBag.pendingReports = reports2Pending.Count().ToString();
            ViewBag.totalDocuments = documents2.Count().ToString();
            ViewBag.pendingDocuments = documents2Pending.Count().ToString();
            return View(company);
        }

        [Breadcrumb(Title = "Kullanıcı Yorumları", AreaName = "Manager", FromAction = "Index")]
        public async Task<IActionResult> CustomerComments()
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
            List<CustomerComment> comments = commentContext.GetDefault(x => x.UserId == Guid.Parse(user.Id) && x.Status != CORE.Entity.Enums.Status.Deleted);

            return View(comments);
        }
        [Breadcrumb(Title = "Yorum Ekleme Formu", AreaName = "Manager", FromAction = "CustomerComments")]
        public IActionResult CreateComment()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CustomerComment customerComment)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
                customerComment.UserId = Guid.Parse(user.Id);
                customerComment.CreatedDate = DateTime.Now;
                customerComment.Status = CORE.Entity.Enums.Status.Pending;
                customerComment.Id = Guid.NewGuid();

                if (commentContext.Add(customerComment))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("CustomerComments");
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    ModelState.AddModelError("", "Yorumunuz sistemimize iletilemedi.");
                }
            }

            return View(customerComment);
        }
        [Breadcrumb(Title = "Yorum Düzenleme Formu", AreaName = "Manager", FromAction = "CustomerComments")]
        public async Task<IActionResult> EditComment(Guid id)
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
            CustomerComment comment = commentContext.GetByID(id);
            if (comment == null || comment.UserId != Guid.Parse(user.Id))
            {
                return NotFound();
            }


            return View(comment);
        }

        [HttpPost]
        public async Task<IActionResult> EditComment(CustomerComment customerComment)
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
            CustomerComment _comment = commentContext.GetByID(customerComment.Id);
            if (_comment == null || _comment.UserId != Guid.Parse(user.Id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _comment.Comment = customerComment.Comment;
                _comment.Status = CORE.Entity.Enums.Status.Pending;

                if (commentContext.Update(_comment))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("CustomerComments");
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    ModelState.AddModelError("", "Yorumunuz güncellenemedi.");
                }
            }

            return View(customerComment);
        }


        public async Task<IActionResult> RemoveComment(Guid id)
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
            CustomerComment comment = commentContext.GetByID(id);
            if (comment == null || comment.UserId != Guid.Parse(user.Id))
            {
                return NotFound();
            }
            else
            {

                if (commentContext.Remove(comment))
                {
                    TempData["returnMessage"] = "success";
                }
                else
                {
                    TempData["returnMessage"] = "error";
                }
            }

            return RedirectToAction("CustomerComments");
        }

        [Breadcrumb(Title = "Firma Detayı", AreaName = "Manager", FromAction = "Index")]
        public async Task<IActionResult> DetailCompany(int page = 1, int pageSize = 3)
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
            Guid companyId = user.CompanyId ?? new Guid();
            Company CompanyInfo = comContext.GetByID(companyId);

            IQueryable<Order> Orders = orderContext.GetDefault(x => x.CompanyId == companyId).OrderByDescending(x=>x.OrderDate).AsQueryable();
            IQueryable<CompanyPackage> packages = packageContext.GetAll().AsQueryable();

            //InnerJoin LinQ Başlangıç
            Orders = Orders.Join(packages, Order => Order.CompanyPackageId, CompanyPackage => CompanyPackage.Id, (Order, CompanyPackage) => Order).ToList().AsQueryable();
            //InnerJoin LinQ Bitiş


            IList<AppUser> employeeList = await userManager.GetUsersInRoleAsync("Çalışan");
            IList<AppUser> managerList = await userManager.GetUsersInRoleAsync("Şirket Temsilcisi");

            int _employeeCount = employeeList.Where(x => x.CompanyId == companyId).ToList().Count();
            int _managerCount = managerList.Where(x => x.CompanyId == companyId).ToList().Count();
            return View(Tuple.Create<Company, IPagedList<Order>, int, int>(CompanyInfo, Orders.ToPagedList(page,pageSize), _employeeCount, _managerCount));

        }
        [Breadcrumb(Title = "Firma Düzenleme Formu", AreaName = "Manager", FromAction = "DetailCompany")]

        public async Task<IActionResult> EditCompany()
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
            Guid companyId = user.CompanyId ?? new Guid();
            Company Company = comContext.GetByID(companyId);
            if (Company == null)
            {
                return RedirectToAction("Companies", "Dashboard");
            }


            return View(Company.Adapt<CompanyVM>());
        }

        [HttpPost]
        public async Task<IActionResult> EditCompany(CompanyVM companyVM, List<IFormFile> Photo)
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
            Guid companyId = user.CompanyId ?? new Guid();
            Company company = comContext.GetByID(companyId);
            string imgPath = Upload.ImageUpload(Photo, _hostingEnv, out bool imgResult);
            if (imgResult)
            {
                company.Photo = imgPath;
            }

            if (ModelState.IsValid)
            {
                company.CompanyName = companyVM.CompanyName;
                company.CompanyAdress = companyVM.CompanyAdress;
                company.CompanyPhone = companyVM.CompanyPhone;

                if (comContext.Update(company))
                {
                    TempData["returnMessage"] = "success";
                }
                else
                {
                    TempData["returnMessage"] = "error";
                }
                return RedirectToAction("DetailCompany");
            }

            return View(companyVM);
        }
        [Breadcrumb(Title = "Firma Silme Formu", AreaName = "Manager", FromAction = "DetailCompany")]

        public IActionResult RemoveCompany()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCompany(RemoveCompanyVM removeCompanyVM)
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
            Guid companyId = user.CompanyId ?? new Guid();
            Company company = comContext.GetByID(companyId);

            if (ModelState.IsValid)
            {
                //Firma işlemleri
                company.DeactivationTime = DateTime.Now;
                company.DeactivationReason = removeCompanyVM.DeactivationReason;
                company.Status = CORE.Entity.Enums.Status.Deleted;
                comContext.Update(company);
                // Firma kapatıldı

                //Kullanıcı işlemleri
                List<AppUser> users = userManager.Users.Where(x=>x.CompanyId==company.Id && x.Status!=CORE.Entity.Enums.Status.Deleted).ToList();
                foreach (AppUser item in users)
                {
                    item.DeactivationTime = DateTime.Now;
                    item.Status = CORE.Entity.Enums.Status.Deleted;
                    item.DeactivationReason = "Firma bu hesabı sonlandırdı.";
                    item.LockoutEnabled = true;
                    item.LockoutEnd = DateTime.Now.AddDays(1);
                    await userManager.UpdateAsync(item); // Kullanıcı hesabına sonlandırma bilgileri işlendi.
                    await userManager.UpdateSecurityStampAsync(item);                   
                   
                }

                await signInManager.SignOutAsync();
                TempData["returnMessage"] = "success";
            }
            else
            {
                TempData["returnMessage"] = "error";
            }

            
            return View(removeCompanyVM);
        }
        [Breadcrumb(Title = "Onay Bekleyen Dosyalar", AreaName = "Manager", FromAction = "Index")]
        public async Task<IActionResult> PendingDocuments()
        {
            AppUser manager = await userManager.FindByNameAsync(User.Identity.Name);

            List<AppUser> users = userManager.Users.Where(x => x.CompanyId == manager.CompanyId && x.Status != CORE.Entity.Enums.Status.Deleted).ToList();
            List<UserDocument> documents = userDocContext.GetDefault(x=>x.Status==CORE.Entity.Enums.Status.Pending).ToList();

            var userdocs = from doc in documents
                          join user in users on doc.UserId.ToString() equals user.Id
                          select new
                          {
                              doc,
                              user
                          };

            List<UserDocJoin> userDocList = new List<UserDocJoin>();
            foreach (var item in userdocs)
            {
                userDocList.Add(new() { 
                    Document=item.doc,
                    AppUser = item.user
                } );
            }

            return View(userDocList);
        }

        public IActionResult ApproveDocument(Guid id)
        {
            UserDocument document = userDocContext.GetByID(id);
            if (document==null)
            {
                return NotFound();
            }

            document.Status = CORE.Entity.Enums.Status.Actived;
            

            if (userDocContext.Update(document))
            {
                TempData["returnMessage"] = "success";
            }
            else
            {
                TempData["returnMessage"] = "error";
            }

            return RedirectToAction("PendingDocuments");
        }

        public IActionResult RemoveDocument(Guid id)
        {
            UserDocument document = userDocContext.GetByID(id);
            if (document == null)
            {
                return NotFound();
            }

            document.Status = CORE.Entity.Enums.Status.Deleted;
            if (userDocContext.Update(document))
            {
                TempData["returnMessage"] = "success";
            }
            else
            {
                TempData["returnMessage"] = "error";
            }

            return RedirectToAction("PendingDocuments");
        }


    }

}
