using BilgeIK.CORE.Service;
using BilgeIK.MODEL.Entities;
using BilgeIK.SERVICE.Helper;
using BilgeIK.WEBUI.ViewModels;
using Mapster;
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
    public class EmployeesController : BaseController
    {
        private readonly ICoreService<Definition> defContext;
        private readonly ICoreService<Company> comContext;
        private readonly ICoreService<Order> comOrder;
        private readonly ICoreService<CompanyPackage> comPacks;
        private readonly ICoreService<UserDocument> userDoc;
        private readonly ICoreService<Leave> userLeave;
        private readonly ICoreService<Report> userReport;
        private readonly ICoreService<SalaryBonus> userSalaryBonus;
        private IHostingEnvironment _hostingEnv;
        public EmployeesController(ILogger<ManagerDashboardController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, ICoreService<Definition> defContext, ICoreService<Company> comContext, ICoreService<Order> comOrder, IHostingEnvironment hostingEnv, ICoreService<CompanyPackage> comPacks, ICoreService<UserDocument> userDoc, ICoreService<Leave> userLeave, ICoreService<Report> userReport, ICoreService<SalaryBonus> userSalaryBonus) : base(logger, userManager, signInManager, roleManager)
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
        }


        [Breadcrumb(Title = "Çalışanlar", AreaName = "Manager", FromController = typeof(ManagerDashboardController))]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
        {
            AppUser loginedManager = await userManager.FindByNameAsync(User.Identity.Name);
            Guid companyId = loginedManager.CompanyId ?? new Guid();

            List<AppUser> employeeList = userManager.Users.Where(x => x.CompanyId == companyId).ToList();
            var list = new List<UserListVM>();

            foreach (AppUser user in employeeList)
            {

                list.Add(new UserListVM()
                {
                    Id = user.Id,
                    Photo = user.Photo,
                    FirstName=user.FirstName,
                    LastName=user.LastName,
                    UserEmail = user.Email,
                    JobTitle = user.JobTitle,
                    Status = user.Status,
                    EmailConfirmed = user.EmailConfirmed,
                    Roles = await userManager.GetRolesAsync(user)
                });

            }

            return View(list.AsQueryable().ToPagedList(page, pageSize));
        }

        [Breadcrumb(Title = "Çalışan Ekleme Formu", AreaName = "Manager", FromAction = "Index")]
        public IActionResult AddEmployee()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(UserVM userVM, string UserRole, List<IFormFile> Photo)
        {
            if (ModelState.IsValid)
            {
                AppUser manager = await userManager.FindByNameAsync(User.Identity.Name); //Manager

                if (userManager.Users.Any(u=>u.PhoneNumber==userVM.PhoneNumber))
                {
                    ModelState.AddModelError("", "Bu telefon numarasını başka bir üye kullanmaktadır.");
                    return View(userVM);
                }

                if (userManager.Users.Any(u => u.NationalIDCardNumber == userVM.NationalIDCardNumber))
                {
                    ModelState.AddModelError("", "Bu T.C. Kimlik numarasını başka bir üye kullanmaktadır.");
                    return View(userVM);
                }

                AppUser user = new AppUser();
                user = userVM.Adapt<AppUser>();

                user.Id = Guid.NewGuid().ToString();
                user.CompanyId = manager.CompanyId;

                string imgPath = Upload.ImageUpload(Photo, _hostingEnv, out bool imgResult);
                if (imgResult)
                {
                    user.Photo = imgPath;
                }

                IdentityResult result = await userManager.CreateAsync(user, userVM.NationalIDCardNumber);

                if (result.Succeeded)
                {

                    if (UserRole == "0")
                    {
                        IdentityResult result2 = await userManager.AddToRoleAsync(user, "Çalışan");
                        if (!result2.Succeeded)
                        {
                            AddModelError(result2);
                        }
                    }
                    else
                    {
                        IdentityResult result2 = await userManager.AddToRoleAsync(user, "Şirket Temsilcisi");
                        if (!result2.Succeeded)
                        {
                            AddModelError(result2);
                        }
                    }

                    string confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    string link = Url.Action("ConfirmEmail", "Auth", new
                    {
                        Area="",
                        userId = user.Id,
                        token = confirmationToken
                    }, protocol: HttpContext.Request.Scheme);
                    EmailConfirmation.SendEmail(link, user.Email);
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("Index", "Employees");

                }
                else
                {
                    AddModelError(result);
                }

            }
            TempData["returnMessage"] = "error";
            return View(userVM);
        }
        [Breadcrumb(Title = "Üye Düzenleme Formu", AreaName = "Manager", FromAction = "Index")]
        public async Task<IActionResult> EditEmployee(string id)
        {
            AppUser manager = await userManager.FindByNameAsync(User.Identity.Name); //Manager

            AppUser user = await userManager.FindByIdAsync(id);
            if (user==null || user.CompanyId!=manager.CompanyId)
            {
                return Redirect("Index");
            }

            if (await userManager.IsInRoleAsync(user, "Çalışan"))
            {
                ViewBag.SelectedRole = "0";
            }
            else if(await userManager.IsInRoleAsync(user, "Şirket Temsilcisi"))
            {
                ViewBag.SelectedRole = "1";
            }

            

            return View(user.Adapt<UserVM>());
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(UserVM userVM, string UserRole, List<IFormFile> Photo)
        {

            AppUser user = await userManager.FindByIdAsync(userVM.Id);

            user.NationalIDCardNumber = userVM.NationalIDCardNumber;
            //Kullanıcı adı kontrolü başlangıcı
            if (user.UserName != userVM.UserName)
            {
                if (await userManager.FindByNameAsync(userVM.UserName) != null)
                {
                    ModelState.AddModelError("", "Bu kullanıcı adı kullanılmaktadır.");
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
            user.JobTitle = userVM.JobTitle;
            //Telefon numarası kontrolü başlangıcı           
            if (user.PhoneNumber != userVM.PhoneNumber)
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
            if (user.Email != userVM.Email)
            {
                if (userManager.Users.Any(u => u.Email == userVM.Email))
                {
                    ModelState.AddModelError("", "Bu e-posta adresini başka bir üye kullanmaktadır.");
                    return View(userVM);
                }
                else
                {
                    user.Email = userVM.Email;
                    user.EmailConfirmed = userVM.EmailConfirmed;
                }
            }

            

            string imgPath = Upload.ImageUpload(Photo, _hostingEnv, out bool imgResult);
            if (imgResult)
            {
                user.Photo = imgPath;
            }

            if (ModelState.IsValid)
            {


                if (UserRole == "0")
                {

                    if (await userManager.IsInRoleAsync(user, "Şirket Temsilcisi"))
                    {
                        /*
                         Selected ile tercih Çalışan olarak geldiyse mevcut yetkisinin Şirket Temsilcisi olup olmadığı kontrol edilir. Şirket Temsilcisi ise ilgili Rolden çıkarılır ve Çalışan rolüne eklenir.
                         */
                        await userManager.RemoveFromRoleAsync(user, "Şirket Temsilcisi");
                        await userManager.AddToRoleAsync(user, "Çalışan");
                    }
                }
                else
                {
                    if (await userManager.IsInRoleAsync(user, "Çalışan"))
                    {
                        /*
                         Selected ile tercih Şirket Temsilcisi olarak geldiyse mevcut yetkisinin Çalışan olup olmadığı kontrol edilir. Çalışan ise ilgili Rolden çıkarılır ve Şirket Temsilcisi rolüne eklenir.
                         */
                        await userManager.RemoveFromRoleAsync(user, "Çalışan");
                        await userManager.AddToRoleAsync(user, "Şirket Temsilcisi");
                    }
                }

                await userManager.UpdateAsync(user);
                TempData["returnMessage"] = "success";
                return RedirectToAction("DetailsEmployee", "Employees", new { Id = user.Id });
            }
            TempData["returnMessage"] = "error";
            return View(userVM);

        }

        public async Task<IActionResult> RemoveEmployee(string id)
        {
            AppUser manager = await userManager.FindByNameAsync(User.Identity.Name); //Manager
            AppUser user = await userManager.FindByIdAsync(id);

            if (user==null || user.CompanyId != manager.CompanyId)
            {
                TempData["returnMessage"] = "false";
                return RedirectToAction("Index");
            }

            user.Status = CORE.Entity.Enums.Status.Deleted;
            await userManager.UpdateAsync(user);
            TempData["returnMessage"] = "success";
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> EmployeeDocumentSender(string Name, string UserId, List<IFormFile> Files)
        {
            AppUser manager =await userManager.FindByNameAsync(User.Identity.Name); // Yetki ilişkisi için çekildi

            if (userManager.Users.Any(u => u.Id == UserId && u.CompanyId==manager.CompanyId))
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
                        TempData["message"] = "Kullanıcı dosyası sistemimizde oluşturulamadı!";
                    }
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    TempData["message"] = "İstenen dosya pdf,doc,docx,txt,xls,xlsx formatında yükleme yapmalısınız!";
                }
                return RedirectToAction("DetailsEmployee", "Employees", new { Id = UserId });
            }
            else
            {
                TempData["returnMessage"] = "error";
                return RedirectToAction("Index", "Employees");
            }

        }
        [Breadcrumb(Title = "Özlük Dosyası Düzenleme Formu", AreaName = "Manager", FromAction = "Index")]
        public IActionResult EditEmployeeDocument(Guid id)
        {
            UserDocument document = userDoc.GetByID(id);

            return View(document.Adapt<UserDocVM>());
        }

        [HttpPost]
        public IActionResult EditEmployeeDocument(UserDocVM userDocVM)
        {
            // TODO: Yetki
            UserDocument document = userDoc.GetByID(Guid.Parse(userDocVM.Id));

            if (document == null)
            {
                return RedirectToAction("Index","Employees");
            }

            if (ModelState.IsValid)
            {
                document.Name = userDocVM.Name;
                document.Status = userDocVM.Status;
                if (userDoc.Update(document))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("DetailsEmployee", "Employees", new { Id = document.UserId });
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    return View(userDocVM);
                }
            }

            return View(userDocVM);

        }

        public IActionResult RemoveEmployeeDocument(Guid id)
        {
            // TODO: Yetki
            UserDocument document = userDoc.GetByID(id);
            if (document == null)
            {
                return RedirectToAction("Index", "Employees");
            }

            if (userDoc.Remove(document))
            {
                TempData["returnMessage"] = "success";
            }
            else
            {
                TempData["returnMessage"] = "error";
            }
            return RedirectToAction("DetailsEmployee", "Employees", new { Id = document.UserId });
        }
        [Breadcrumb(Title = "İzin Ekleme Formu", AreaName = "Manager", FromAction = "Index")]
        public async Task<IActionResult> AddLeave(string Id)
        {
            //TODO MANAGER KONTROL
            AppUser user = await userManager.FindByIdAsync(Id);
            if (user==null)
            {
                return RedirectToAction("Index");
            }
            LeaveVM leave = new LeaveVM();
            leave.UserId = Guid.Parse(Id);
            leave.FromDate = DateTime.Now;
            leave.ToDate = DateTime.Now;

            leave.Definition= defContext.GetDefault(x=>x.DefinitionType==CORE.Entity.Enums.DefinitionType.Leave); //Tanımlamaları doldurduk

            return View(leave);
        }

        [HttpPost]
        public async Task<IActionResult> AddLeave(LeaveVM leaveVM)
        {
            Leave leave = leaveVM.Adapt<Leave>();
            leave.Id = Guid.NewGuid();

            if (ModelState.IsValid)
            {
                if (userLeave.Add(leave))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("DetailsEmployee", "Employees", new { Id = leave.UserId });
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    ModelState.AddModelError("", "İzin ekleme esnasında hata ile karşılaşıldı.");
                }

            }
            return View(leaveVM);
        }
        [Breadcrumb(Title = "İzin Detayı", AreaName = "Manager", FromAction = "Index")]
        public async Task<IActionResult> DetailsEmployeeLeave(Guid id)
        {
            LeaveVM leave = userLeave.GetByID(id).Adapt<LeaveVM>();
            leave.AppUser = await userManager.FindByIdAsync(leave.UserId.ToString());
            ViewBag.IzinTuru = defContext.GetByID(leave.DefinitionId).Name.ToString();
            return View(leave);
        }
        [Breadcrumb(Title = "İzin Düzenleme Formu", AreaName = "Manager", FromAction = "Index")]

        public IActionResult EditEmployeeLeave(Guid id)
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
        public IActionResult EditEmployeeLeave(LeaveVM leaveVM)
        {
            Leave leave = leaveVM.Adapt<Leave>();
            
            if (ModelState.IsValid)
            {
                if (userLeave.Update(leave))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("DetailsEmployee", "Employees", new { Id = leave.UserId });
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    ModelState.AddModelError("", "İzin güncelleme esnasında hata ile karşılaşıldı.");
                }

            }
            return View(leaveVM);
        }

        public IActionResult RemoveEmployeeLeave(Guid id)
        {
            Leave leave = userLeave.GetByID(id);
            if (leave==null)
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

            return RedirectToAction("DetailsEmployee", "Employees", new { Id = leave.UserId });

        }

        [Breadcrumb(Title = "Rapor Ekleme Formu", AreaName = "Manager", FromAction = "Index")]
        public async Task<IActionResult> AddReport(string Id)
        {
            //TODO MANAGER KONTROL
            AppUser user = await userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            ReportVM report = new ReportVM();
            report.UserId = Guid.Parse(Id);
            report.FromDate = DateTime.Now;
            report.ToDate = DateTime.Now;

            report.Definition = defContext.GetDefault(x => x.DefinitionType == CORE.Entity.Enums.DefinitionType.Report);
            //Tanımlamaları doldurduk

            return View(report);
        }

        [HttpPost]
        public async Task<IActionResult> AddReport(ReportVM reportVM)
        {
            //Todo yetki kontrolü
            if (ModelState.IsValid)
            {
                Report report = new Report();
                report = reportVM.Adapt<Report>();
                report.Id = Guid.NewGuid();

                if (userReport.Add(report))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("DetailsEmployee", "Employees", new { Id = report.UserId });
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    ModelState.AddModelError("", "Rapor ekleme esnasında hata ile karşılaşıldı.");
                }
            }
            return View(reportVM);
        }
        [Breadcrumb(Title = "Rapor Detayı", AreaName = "Manager", FromAction = "Index")]
        public async Task<IActionResult> DetailsEmployeeReport(Guid id)
        {
            Report report = userReport.GetByID(id);
            if (report==null)
            {
                return NotFound();
            }

            report.AppUser = await userManager.FindByIdAsync(report.UserId.ToString());
            ViewBag.RaporTuru = defContext.GetByID(report.DefinitionId).Name.ToString();

            return View(report.Adapt<ReportVM>());
        }
        [Breadcrumb(Title = "Rapor Düzenleme Formu", AreaName = "Manager", FromAction = "Index")]
        public IActionResult EditEmployeeReport(Guid id)
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
        public IActionResult EditEmployeeReport(ReportVM reportVM)
        {
            Report report = reportVM.Adapt<Report>();

            if (ModelState.IsValid)
            {
                if (userReport.Update(report))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("DetailsEmployee", "Employees", new { Id = report.UserId });
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    ModelState.AddModelError("", "Rapor güncelleme esnasında hata ile karşılaşıldı.");
                }

            }
            return View(reportVM);
        }

        public IActionResult RemoveEmployeeReport(Guid id)
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

            return RedirectToAction("DetailsEmployee", "Employees", new { Id = report.UserId });

        }

        [Breadcrumb(Title = "Ek Maaş Ekleme Formu", AreaName = "Manager", FromAction = "Index")]
        public async Task<IActionResult> AddSalaryBonus(string Id)
        {
            AppUser manager = await userManager.FindByNameAsync(User.Identity.Name);
            AppUser user = await userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            SalaryBonusVM salaryBonusVM = new SalaryBonusVM();
            salaryBonusVM.UserId = Guid.Parse(user.Id);
            salaryBonusVM.Date = DateTime.Now;
            salaryBonusVM.ApproverUserId = Guid.Parse(manager.Id);
            return View(salaryBonusVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddSalaryBonus(SalaryBonusVM salaryBonusVM)
        {
            AppUser manager = await userManager.FindByNameAsync(User.Identity.Name);
            if(ModelState.IsValid)
            {
                SalaryBonus salaryBonus = salaryBonusVM.Adapt<SalaryBonus>();
                salaryBonus.Id = Guid.NewGuid();
                salaryBonus.ApproverUserId = Guid.Parse(manager.Id);

                if (userSalaryBonus.Add(salaryBonus))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("DetailsEmployee", "Employees", new { Id = salaryBonus.UserId });
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    ModelState.AddModelError("", "Ek ödeme kaydı ekleme esnasında hata ile karşılaşıldı.");
                }

            }

            return View(salaryBonusVM);
        }
        [Breadcrumb(Title = "Ek Maaş Detayı", AreaName = "Manager", FromAction = "Index")]
        public async Task<IActionResult> DetailsEmployeeSBonus(Guid id)
        {
            SalaryBonusVM salaryBonusVM = userSalaryBonus.GetByID(id).Adapt<SalaryBonusVM>();
            salaryBonusVM.AppUser = await userManager.FindByIdAsync(salaryBonusVM.UserId.ToString());
            salaryBonusVM.ApproverUser = await userManager.FindByIdAsync(salaryBonusVM.ApproverUserId.ToString());

            return View(salaryBonusVM);
        }
        [Breadcrumb(Title = "Ek Maaş Kaydını Düzenleme Formu", AreaName = "Manager", FromAction = "Index")]
        public IActionResult EditEmployeeSBonus(Guid id)
        {
            SalaryBonusVM salaryBonusVM = userSalaryBonus.GetByID(id).Adapt<SalaryBonusVM>();
            if (salaryBonusVM==null)
            {
                return NotFound();
            }

            return View(salaryBonusVM);
        }

        [HttpPost]
        public IActionResult EditEmployeeSBonus(SalaryBonusVM salaryBonusVM)
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
                return RedirectToAction("DetailsEmployee", "Employees", new { Id = salaryBonusVM.UserId });
            }

            return View(salaryBonusVM);
        }

        public IActionResult RemoveEmployeeSBonus(Guid id)
        {
            SalaryBonus salaryBonus = userSalaryBonus.GetByID(id);
            if (salaryBonus==null)
            {
                return NotFound();
            }

            salaryBonus.Status = CORE.Entity.Enums.Status.Deleted;
            userSalaryBonus.Update(salaryBonus);

            return RedirectToAction("DetailsEmployee", "Employees", new { Id = salaryBonus.UserId });
        }


        [Breadcrumb(Title = "Çalışan Profili", AreaName = "Manager", FromAction = "Index")]
        public async Task<IActionResult> DetailsEmployee(string id)
        {
            AppUser manager = await userManager.FindByNameAsync(User.Identity.Name); // Yetki için manager çağrıldı

            AppUser user = userManager.Users.Where(x => x.Id == id && x.CompanyId == manager.CompanyId).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Employees");
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
            leaves = leaves.Join(definitions, Leave => Leave.DefinitionId, Definition => Definition.Id, (Leave, Definition) => Leave).OrderByDescending(x => x.FromDate).ToList();
            //InnerJoin LinQ Bitiş


            List<Report> reports = userReport.GetDefault(x => x.UserId == Guid.Parse(user.Id)).TakeLast(10).ToList();
            //InnerJoin LinQ Başlangıç
            reports = reports.Join(definitions, Report => Report.DefinitionId, Definition => Definition.Id, (Report, Definition) => Report).OrderByDescending(x=>x.FromDate).ToList();
            //InnerJoin LinQ Bitiş


            List<SalaryBonus> salaryBonus = userSalaryBonus.GetDefault(x => x.UserId == Guid.Parse(user.Id)).TakeLast(10).ToList();

            return View(Tuple.Create<UserVM, IList<string>, string, List<UserDocument>, List<Leave>, List<Report>, List<SalaryBonus>>(user.Adapt<UserVM>(), role, CompanyName, documents, leaves, reports, salaryBonus));
            return View();
        }
        [Breadcrumb(Title = "İzin Yönetimi", AreaName = "Manager", FromAction = "Index")]
        public async Task<IActionResult> Leaves(int page = 1, int pageSize = 3)
        {
            AppUser manager = await userManager.FindByNameAsync(User.Identity.Name); // Yetki için manager çağrıldı



           var leaves= from izin in userLeave.GetAll()
                        join user in userManager.Users.Where(x => x.CompanyId == manager.CompanyId).ToList() on izin.UserId.ToString() equals user.Id
                        join def in defContext.GetDefault(x => x.DefinitionType == CORE.Entity.Enums.DefinitionType.Leave) on izin.DefinitionId equals def.Id
                        select new
                        {
                            izin,
                            user,
                            def
                        };

            List<LeaveJoinUserVM> izinler = new List<LeaveJoinUserVM>();

            foreach (var item in leaves)
            {
                izinler.Add(new LeaveJoinUserVM{ 
                    AppUser = item.user,
                    Leave = item.izin,
                    Definition = item.def
                });
            }


            return View(izinler.OrderByDescending(x=>x.Leave.FromDate).AsQueryable().ToPagedList(page, pageSize));
        }



        [Breadcrumb(Title = "Rapor Yönetimi", AreaName = "Manager", FromAction = "Index")]
        public async Task<IActionResult> Reports(int page = 1, int pageSize = 3)
        {
            AppUser manager = await userManager.FindByNameAsync(User.Identity.Name); // Yetki için manager çağrıldı



            var reports = from rapor in userReport.GetAll()
                         join user in userManager.Users.Where(x => x.CompanyId == manager.CompanyId).ToList() on rapor.UserId.ToString() equals user.Id
                         join def in defContext.GetDefault(x => x.DefinitionType == CORE.Entity.Enums.DefinitionType.Report) on rapor.DefinitionId equals def.Id
                         select new
                         {
                             rapor,
                             user,
                             def
                         };

            List<ReportJoinUserVM> raporlar = new List<ReportJoinUserVM>();

            foreach (var item in reports)
            {
                raporlar.Add(new ReportJoinUserVM
                {
                    AppUser = item.user,
                    Report = item.rapor,
                    Definition = item.def
                });
            }


            return View(raporlar.OrderByDescending(x=>x.Report.FromDate).AsQueryable().ToPagedList(page, pageSize));
        }




    }
}
