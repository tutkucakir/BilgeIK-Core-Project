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
using Newtonsoft.Json;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.Areas.Personal.Controllers
{
    [Authorize]
    public class PersonalDashboardController : BaseController
    {
        private readonly ICoreService<Definition> defContext;
        private readonly ICoreService<UserDocument> userDoc;
        private readonly ICoreService<Leave> userLeave;
        private readonly ICoreService<Report> userReport;
        private readonly ICoreService<SalaryBonus> userSalaryBonus;
        private readonly ICoreService<SBDocument> sbDocumentContext;
        private IHostingEnvironment _hostingEnv;
        public PersonalDashboardController(ILogger<PersonalDashboardController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, ICoreService<Definition> defContext, ICoreService<UserDocument> userDoc, ICoreService<Leave> userLeave, ICoreService<Report> userReport, ICoreService<SalaryBonus> userSalaryBonus, IHostingEnvironment hostingEnv, ICoreService<SBDocument> sbDocumentContext) : base(logger, userManager, signInManager, roleManager)
        {
            _hostingEnv = hostingEnv;
            this.defContext = defContext;
            this.userDoc = userDoc;
            this.userLeave = userLeave;
            this.userReport = userReport;
            this.userSalaryBonus = userSalaryBonus;
            this.sbDocumentContext = sbDocumentContext;
        }

        [Breadcrumb("Personel Ekranı", AreaName = "Personal")]
        public async Task<IActionResult> Index()
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);

            List<UserDocument> documents = userDoc.GetDefault(x => x.UserId == Guid.Parse(user.Id) && x.Status != CORE.Entity.Enums.Status.Deleted).Take(10).ToList();

            List<Definition> leaveDefinitions = defContext.GetDefault(x=>x.DefinitionType==CORE.Entity.Enums.DefinitionType.Leave);

            List<Definition> reportDefinitions = defContext.GetDefault(x=>x.DefinitionType==CORE.Entity.Enums.DefinitionType.Report);
            
            List<Leave> leaves = userLeave.GetDefault(x=>x.UserId==Guid.Parse(user.Id) && x.Status!=CORE.Entity.Enums.Status.Deleted).Take(10).ToList();

            List<Report> reports = userReport.GetDefault(x=>x.UserId==Guid.Parse(user.Id) && x.Status!=CORE.Entity.Enums.Status.Deleted).Take(10).ToList();

            //InnerJoin LinQ Başlangıç
            leaves = leaves.Join(leaveDefinitions, Leave => Leave.DefinitionId, Definition => Definition.Id, (Leave, Definition) => Leave).ToList();
            //InnerJoin LinQ Bitiş

            //InnerJoin LinQ Başlangıç
            reports = reports.Join(reportDefinitions, Report => Report.DefinitionId, Definition => Definition.Id, (Report, Definition) => Report).ToList();
            //InnerJoin LinQ Bitiş



            List<SalaryBonus> salaryBonus = userSalaryBonus.GetDefault(x=>x.UserId==Guid.Parse(user.Id) && x.Status!=CORE.Entity.Enums.Status.Deleted).Take(10).ToList();
            Holiday holiday = new Holiday();
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage httpResponse = await client.GetAsync(new Uri("https://www.googleapis.com/calendar/v3/calendars/turkish__tr%40holiday.calendar.google.com/events?key=AIzaSyD046GUFHs4HDtHhT6-PSUQeFOBpvHYg3k")).ConfigureAwait(false);
                    string result = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                    holiday = JsonConvert.DeserializeObject<Holiday>(result);
                    holiday.items = holiday.items.Where(x=>x.start.date.Year==DateTime.Now.Year).OrderBy(x => x.start.date).ToArray();
                }
            }
            catch (Exception)
            {
                
            }

            return View(Tuple.Create<UserVM, List<UserDocument>, List<Leave>, List<Report>, List<SalaryBonus>, List<Definition>,Holiday>(user.Adapt<UserVM>(), documents, leaves, reports, salaryBonus, leaveDefinitions, holiday));
        }

        [Breadcrumb("İzin Talep Formu", AreaName = "Personal", FromAction = "Index")]
        public IActionResult AddLeave()
        {
            LeaveVM leave = new LeaveVM();
            leave.FromDate = DateTime.Now;
            leave.Definition = defContext.GetDefault(x => x.DefinitionType == CORE.Entity.Enums.DefinitionType.Leave);


            return View(leave);
        }

        [HttpPost]
        public async Task<IActionResult> AddLeave(LeaveVM leaveVM)
        {

            if (ModelState.IsValid)
            {
                Leave leave = leaveVM.Adapt<Leave>();
                leave.UserId = Guid.Parse((await userManager.FindByNameAsync(User.Identity.Name)).Id);
                if (userLeave.Add(leave))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    ModelState.AddModelError("","İzin talebi eklenirken hata ile karşılaşıldı.");
                    leaveVM.Definition = defContext.GetDefault(x => x.DefinitionType == CORE.Entity.Enums.DefinitionType.Leave);

                    return View(leaveVM);
                }

            }
            leaveVM.Definition = defContext.GetDefault(x => x.DefinitionType == CORE.Entity.Enums.DefinitionType.Leave);

            return View(leaveVM);
        }
        [Breadcrumb("Özlük Dosyası Ekleme Formu", AreaName = "Personal", FromAction = "Index")]
        public IActionResult AddDocument()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDocument(UserDocVM userDocVM, List<IFormFile> Files)
        {

            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByNameAsync(User.Identity.Name);

                string filePath = Upload.FileUpload(Files, _hostingEnv, out bool fileResult);
                if (fileResult)
                {
                    UserDocument userDocument = new UserDocument()
                    {
                        Name = userDocVM.Name,
                        FileUrl = filePath,
                        UserId = Guid.Parse(user.Id),
                        Status = CORE.Entity.Enums.Status.Pending,
                        CreatedDate = DateTime.Now
                    };

                    if (userDoc.Add(userDocument))
                    {
                        TempData["returnMessage"] = "success";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["returnMessage"] = "error";
                        ModelState.AddModelError("", "Kullanıcı dosyası sistemimizde oluşturulamadı!");
                    }
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    TempData["message"] = "İstenen dosya pdf,doc,docx,txt,xls,xlsx formatında yükleme yapmalısınız!";
                    ModelState.AddModelError("", "İstenen dosya pdf,doc,docx,txt,xls,xlsx formatında yükleme yapmalısınız!");
                }
                


            }

            return View(userDocVM);
        }
        [Breadcrumb("Özlük Dosyası Düzenleme Formu", AreaName = "Personal", FromAction = "Index")]
        public async Task<IActionResult> EditDocument(Guid id)
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name); //Login olan kullanıcı verisi
            UserDocument document = userDoc.GetByDefault(x=>x.Id==id && x.UserId==Guid.Parse(user.Id)); // Doğrulanmış veri


            return View(document.Adapt<UserDocVM>());
        }

        [HttpPost]
        public async Task<IActionResult> EditDocument(UserDocVM userDocVM)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByNameAsync(User.Identity.Name); //Login olan kullanıcı verisi
                UserDocument document = userDoc.GetByDefault(x => x.Id == Guid.Parse(userDocVM.Id) && x.UserId == Guid.Parse(user.Id)); // Doğrulanmış veri

                if (document==null)
                {
                    return View(userDocVM);
                }

                document.Name = userDocVM.Name;

                if (userDoc.Update(document))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    ModelState.AddModelError("", "Dosya adı değiştirilemedi!");
                }

            }


            return View(userDocVM);
        }

        public async Task<IActionResult> RemoveDocument(Guid id)
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name); //Login olan kullanıcı verisi
            UserDocument document = userDoc.GetByDefault(x => x.Id == id && x.UserId == Guid.Parse(user.Id)); // Doğrulanmış veri

            userDoc.Remove(document);

            return RedirectToAction("Index");
        }
        [Breadcrumb("İzin Detayı", AreaName = "Personal", FromAction = "Index")]
        public async Task<IActionResult> DetailsLeave(Guid id)
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name); //Login olan kullanıcı verisi
            Leave leave = userLeave.GetByDefault(x=>x.Id==id && x.UserId==Guid.Parse(user.Id));
            Definition definition = defContext.GetByID(leave.DefinitionId);
            ViewBag.izinTuru = definition.Name;
            return View(leave.Adapt<LeaveVM>());
        }

        [Breadcrumb("İzin Düzenleme Formu", AreaName = "Personal", FromAction = "Index")]
        public async Task<IActionResult> EditLeave(Guid id)
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name); //Login olan kullanıcı verisi
            Leave leave = userLeave.GetByDefault(x => x.Id == id && x.UserId == Guid.Parse(user.Id));

            LeaveVM leaveVM = leave.Adapt<LeaveVM>();


            leaveVM.Definition=defContext.GetActive();


            return View(leaveVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditLeave(LeaveVM leaveVM)
        {

            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByNameAsync(User.Identity.Name); //Login olan kullanıcı verisi
                Leave leave = userLeave.GetByDefault(x => x.Id == Guid.Parse(leaveVM.Id) && x.UserId == Guid.Parse(user.Id));
                if (leave!=null)
                {
                    leave.FromDate = leaveVM.FromDate;
                    leave.ToDate = leaveVM.ToDate;
                    leave.PlannedDay = leaveVM.PlannedDay;
                    leave.Notes = leaveVM.Notes;
                    leave.DefinitionId = leaveVM.DefinitionId;
                    if (userLeave.Update(leave))
                    {
                        TempData["returnMessage"] = "success";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["returnMessage"] = "error";
                    }
                }
            }

            leaveVM.Definition = defContext.GetActive();

            return View(leaveVM);
        }

        public async Task<IActionResult> RemoveLeave(Guid id)
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name); //Login olan kullanıcı verisi
            Leave leave = userLeave.GetByDefault(x => x.Id == id && x.UserId == Guid.Parse(user.Id));


            if (userLeave.Remove(leave))
            {
                TempData["returnMessage"] = "success";
            }
            else
            {
                TempData["returnMessage"] = "error";
            }

            return RedirectToAction("Index");
        }
        [Breadcrumb("Rapor Detayı", AreaName = "Personal", FromAction = "Index")]
        public async Task<IActionResult> DetailsReport(Guid id)
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name); //Login olan kullanıcı verisi
            Report report = userReport.GetByDefault(x => x.Id == id && x.UserId == Guid.Parse(user.Id));
            Definition definition = defContext.GetByID(report.DefinitionId);
            ViewBag.raporTuru = definition.Name;
            return View(report.Adapt<ReportVM>());
        }
        [Breadcrumb("Ek Maaş Ekleme Formu", AreaName = "Personal", FromAction = "Index")]
        public async Task<IActionResult> AddSalaryBonus()
        {

            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            SalaryBonusVM salaryBonusVM = new SalaryBonusVM();
            salaryBonusVM.UserId = Guid.Parse(user.Id);
            salaryBonusVM.Date = DateTime.Now;
            salaryBonusVM.ApproverUserId = Guid.Parse(user.Id);
            return View(salaryBonusVM);
        }
        [HttpPost]
        public IActionResult AddSalaryBonus(SalaryBonusVM salaryBonusVM)
        {
            if (ModelState.IsValid)
            {
                Guid newGuid = Guid.NewGuid();
                SalaryBonus salaryBonus = new();
                salaryBonus = salaryBonusVM.Adapt<SalaryBonus>();
                salaryBonus.Status = CORE.Entity.Enums.Status.Pending;
                salaryBonus.Id = newGuid;
                if (userSalaryBonus.Add(salaryBonus))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("DetailsSalaryBonus", new { id=newGuid });
                }
                else
                {
                    TempData["returnMessage"] = "error";
                }
            }


            return View(salaryBonusVM);
        }



        [Breadcrumb("Ek Maaş Detayı", AreaName = "Personal", FromAction = "Index")]
        public async Task<IActionResult> DetailsSalaryBonus(Guid id)
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name); //Login olan kullanıcı verisi
            SalaryBonus bonus = userSalaryBonus.GetByDefault(x => x.Id == id && x.UserId == Guid.Parse(user.Id));
            List<SBDocument> documents = sbDocumentContext.GetDefault(x => x.SalaryBonusId == id);

            return View(Tuple.Create<SalaryBonus, List<SBDocument>>(bonus, documents));
        }
        [Breadcrumb("Ek Maaş Dosya Yükleme Formu", AreaName = "Personal", FromAction = "Index")]
        public IActionResult AddSBDocument(Guid id)
        {
            SalaryBonus sb = userSalaryBonus.GetByID(id);
            if (sb==null)
            {
                return NotFound();
            }

            SBDocumentVM model = new();
            model.SalaryBonusId = id;


            return View(model);
        }

        [HttpPost]
        public IActionResult AddSBDocument(SBDocumentVM sBDocumentVM, List<IFormFile> File)
        {

            if (ModelState.IsValid)
            {
                string filePath = Upload.ImageAndFileUpload(File, _hostingEnv, out bool fileResult);
                if (fileResult)
                {
                    Guid newGuid = Guid.NewGuid();
                    SBDocument doc = new() {
                         SalaryBonusId = sBDocumentVM.SalaryBonusId,
                         File = filePath,
                         Id = newGuid,
                         Description = sBDocumentVM.Description

                    };

                    if (sbDocumentContext.Add(doc))
                    {
                        TempData["returnMessage"] = "success";
                        return RedirectToAction("DetailsSalaryBonus",new { id= sBDocumentVM.SalaryBonusId });
                    }
                    else
                    {
                        TempData["returnMessage"] = "error";
                        ModelState.AddModelError("", "Kullanıcı dosyası sistemimizde oluşturulamadı!");
                    }
                }
                else
                {
                    TempData["message"] = "İstenen dosya pdf,doc,docx,txt,xls,xlsx,jpg,png  formatında yükleme yapmalısınız!";
                    ModelState.AddModelError("", "İstenen dosya pdf,doc,docx,txt,xls,xlsx,jpg,png formatında yükleme yapmalısınız!");
                }



            }

            return View(sBDocumentVM);
        }

    }
}
