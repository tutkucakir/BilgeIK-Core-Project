using BilgeIK.CORE.Service;
using BilgeIK.MODEL.Entities;
using BilgeIK.SERVICE.Base;
using BilgeIK.WEBUI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using BilgeIK.SERVICE.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using PagedList.Core;

namespace BilgeIK.WEBUI.Areas.Administrator.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly ICoreService<Definition> defContext;
        private readonly ICoreService<Company> comContext;
        private readonly ICoreService<Order> comOrder;
        private readonly ICoreService<CompanyPackage> comPacks;
        private readonly ICoreService<BankAccount> bankContext;
        private readonly ICoreService<Contact> contactContext;
        private readonly ICoreService<CustomerComment> commentContext;
        private IHostingEnvironment _hostingEnv;
        public DashboardController(ILogger<DashboardController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, ICoreService<Definition> defContext, ICoreService<Company> comContext, ICoreService<Order> comOrder, IHostingEnvironment hostingEnv, ICoreService<CompanyPackage> comPacks, ICoreService<BankAccount> bankContext, ICoreService<Contact> contactContext, ICoreService<CustomerComment> commentContext) : base(logger, userManager, signInManager, roleManager)
        {
            this.defContext = defContext;
            this.comContext = comContext;
            this.comOrder = comOrder;
            _hostingEnv = hostingEnv;
            this.comPacks = comPacks;
            this.bankContext = bankContext;
            this.contactContext = contactContext;
            this.commentContext = commentContext;
        }

        [Breadcrumb("Pano", AreaName = "Administrator")]
        public IActionResult Index()
        {
            ViewBag.companyCounter = comContext.GetActive().Count();
            ViewBag.pendingCompanyCounter = comContext.GetDefault(x => x.Status == CORE.Entity.Enums.Status.Pending).Count();
            ViewBag.totalActiveUser = userManager.Users.Count(x => x.Status == CORE.Entity.Enums.Status.Actived);
            ViewBag.totalPendingUser = userManager.Users.Count(x => x.Status == CORE.Entity.Enums.Status.Pending);
            ViewBag.pendingOrders = comOrder.GetDefault(x => x.Status == CORE.Entity.Enums.Status.Pending).Count();
            ViewBag.totalOrders = comOrder.GetDefault(x => x.Status == CORE.Entity.Enums.Status.Actived).Count();
            ViewBag.pendingContacts = contactContext.GetDefault(x => x.Status == CORE.Entity.Enums.Status.Pending).Count();
            ViewBag.pendingComments = commentContext.GetDefault(x => x.Status == CORE.Entity.Enums.Status.Pending).Count();
            return View();
        }

        [Breadcrumb("Tanımlamalar", AreaName = "Administrator")]
        public IActionResult Definitions()
        {
            DefinitionTablesVM Definitions = new DefinitionTablesVM();
            Definitions.Izinler = defContext.GetDefault(x=>x.DefinitionType==CORE.Entity.Enums.DefinitionType.Leave && x.Status== CORE.Entity.Enums.Status.Actived);
            Definitions.Raporlar = defContext.GetDefault(x=>x.DefinitionType==CORE.Entity.Enums.DefinitionType.Report && x.Status == CORE.Entity.Enums.Status.Actived);
            return View(Definitions);
        }

        [Breadcrumb(AreaName ="Administrator", FromAction = "Definitions", Title = "Tanımlama Ekle")]
        public IActionResult AddDefinition()
        {
            DefinitionVM def = new DefinitionVM();
            return View(def);
        }

        [HttpPost]
        public IActionResult AddDefinition(DefinitionVM def)
        {
            if (ModelState.IsValid)
            {
                Definition definition = new Definition();
                definition.Name = def.Name;
                definition.DefinitionType = def.DefinitionTypes;
                definition.Status = CORE.Entity.Enums.Status.Actived;

                if (defContext.Add(definition))
                {
                    TempData["returnMessage"] = "success";
                }
                else
                {
                    TempData["returnMessage"] = "error";
                }
                return RedirectToAction("Definitions","Dashboard");
            }

            return View(def);
        }

        [Breadcrumb(AreaName = "Administrator", FromAction = "Definitions", Title = "Düzenle")]
        public IActionResult EditDefinition(Guid id)
        {
            Definition def = defContext.GetByID(id);
            if (def==null)
            {
                return RedirectToAction("Definitions","Dashboard");
            }
            return View(def.Adapt<DefinitionVM>());
        }

        [HttpPost]
        public IActionResult EditDefinition(DefinitionVM defVM)
        {
            Definition def = defContext.GetByID(Guid.Parse(defVM.Id));
            if (def!=null)
            {
                def.Name = defVM.Name;
                def.DefinitionType = defVM.DefinitionTypes;
                if (defContext.Update(def))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("Definitions", "Dashboard");
                }
                else
                {
                    TempData["returnMessage"] = "error";
                }
            }

            return View(defVM);

        }


        public IActionResult RemoveDefinition(Guid id)
        {
            Definition def = defContext.GetByID(id);
            if (def!=null)
            {

                if (defContext.Remove(def))
                {
                    TempData["returnMessage"] = "success";
                }
                else
                {
                    TempData["returnMessage"] = "error";
                }
            }

            return RedirectToAction("Definitions", "Dashboard");
        }

        [Breadcrumb(AreaName = "Administrator", Title = "Firmalar")]
        public IActionResult Companies(int page = 1, int pageSize = 3)
        {
            List<Company> companies = comContext.GetAll().ToList();
            return View(companies.AsQueryable().ToPagedList(page, pageSize));
        }

        [Breadcrumb(AreaName = "Administrator", FromAction = "Companies", Title = "Firma Detayı")]
        public async Task<IActionResult> DetailCompany(Guid id)
        {
            Company CompanyInfo = comContext.GetByID(id);
            List<Order> Orders = comOrder.GetDefault(x => x.CompanyId == id);
            List<CompanyPackage> packages = comPacks.GetAll();
            //InnerJoin LinQ Başlangıç
            Orders = Orders.Join(packages, Order => Order.CompanyPackageId, CompanyPackage => CompanyPackage.Id, (Order, CompanyPackage) => Order).ToList();
            //InnerJoin LinQ Bitiş



            IList<AppUser> employeeList = await userManager.GetUsersInRoleAsync("Çalışan");
            IList<AppUser> managerList = await userManager.GetUsersInRoleAsync("Şirket Temsilcisi");

            int _employeeCount = employeeList.Where(x => x.CompanyId == id).ToList().Count();
            int _managerCount = managerList.Where(x => x.CompanyId == id).ToList().Count();
            return View(Tuple.Create<Company, List<Order>, int, int>(CompanyInfo, Orders, _employeeCount, _managerCount));
        }

        [Breadcrumb(AreaName = "Administrator", FromAction = "Companies", Title = "Firma Düzenleme Sayfası")]
        public IActionResult EditCompany(Guid id)
        {
            Company company = comContext.GetByID(id);
            if (company==null)
            {
                return RedirectToAction("Companies","Dashboard");
            }
            

            return View(company.Adapt<CompanyVM>());
        }

        [HttpPost]
        public IActionResult EditCompany(CompanyVM companyVm, List<IFormFile> Photo)
        {
            var companyId = companyVm.Id ?? Guid.NewGuid().ToString();
            Company company = comContext.GetByID(Guid.Parse(companyId));
            string imgPath = Upload.ImageUpload(Photo, _hostingEnv, out bool imgResult);
            if (imgResult)
            {
                company.Photo = imgPath;
            }
            else
            {
                Company c1 = comContext.GetByID(Guid.Parse(companyVm.Id));
                company.Photo = c1.Photo;
            }

            if (ModelState.IsValid)
            {
                company.CompanyName = companyVm.CompanyName;
                company.CompanyPhone = companyVm.CompanyPhone;
                company.CompanyAdress = companyVm.CompanyAdress;
                company.RegisteredDate = companyVm.RegisteredDate;
                company.UpdatedDate = companyVm.UpdatedDate;
                company.ExpiresDate = companyVm.ExpiresDate;
                company.Status = companyVm.Status;

                if (comContext.Update(company))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("DetailCompany", "DashBoard", new { Id = companyVm.Id });
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    ModelState.AddModelError("","Hata meydana geldi.");
                    return View(companyVm);
                }
                
            }

            return View(companyVm);
        }

        [Breadcrumb(AreaName = "Administrator", Title = "Firma Üyelik Paketleri")]
        public IActionResult Packages()
        {
            return View(comPacks.GetAll());
        }

        [Breadcrumb(AreaName ="Administrator", FromAction ="Packages", Title ="Üyelik Paketi Ekle")]
        public IActionResult AddPackage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPackage(CompanyPackageVM packageVM)
        {
            if (ModelState.IsValid)
            {
                CompanyPackage package = packageVM.Adapt<CompanyPackage>();
                package.Id = new Guid();
                if (comPacks.Add(package))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("Packages","Dashboard");
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    ModelState.AddModelError("Paket eklenemedi","");
                }

            }

            return View(packageVM);
        }
        [Breadcrumb(Title = "Paket Düzenleme Formu", AreaName = "Administrator", FromAction = "Packages")]
        public IActionResult EditPackage(Guid id)
        {
            CompanyPackage package = comPacks.GetByID(id);
            if (package==null)
            {
                return RedirectToAction("Packages", "Dashboard");
            }

            return View(package.Adapt<CompanyPackageVM>());
        }

        [HttpPost]
        public IActionResult EditPackage(CompanyPackageVM packageVM)
        {
            if (ModelState.IsValid)
            {
                CompanyPackage package = packageVM.Adapt<CompanyPackage>();
                if (comPacks.Update(package))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("Packages", "Dashboard");
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    ModelState.AddModelError("Güncelleme işlemi esnasında hata oluştu!","");
                }
            }


            return View(packageVM);
        }

        public IActionResult RemovePackage(Guid id)
        {
            CompanyPackage package = comPacks.GetByID(id);

            if (comPacks.Remove(package))
            {
                TempData["returnMessage"] = "success";
            }
            else
            {
                TempData["returnMessage"] = "error";
            }

            return RedirectToAction("Packages", "Dashboard");
        }

        [Breadcrumb(AreaName = "Administrator", FromAction = "Packages", Title = "Paket Detayı")]
        public IActionResult DetailPackage(Guid id)
        {

            CompanyPackage package = comPacks.GetByID(id);
            List<Order> orders = comOrder.GetDefault(x => x.CompanyPackageId == id);

            return View(Tuple.Create<CompanyPackage,List<Order>>(package, orders));
        }
        [Breadcrumb("Banka Hesapları", AreaName = "Administrator")]
        public IActionResult BankAccounts()
        {
            List<BankAccount> accounts = bankContext.GetAll();

            return View(accounts);
        }
        [Breadcrumb(AreaName = "Administrator", FromAction = "BankAccounts", Title = "Banka Hesabı Oluşturun")]
        public IActionResult AddBankAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBankAccount(BankAccountVM bankAccountVM)
        {
            if (ModelState.IsValid)
            {
                BankAccount account = bankAccountVM.Adapt<BankAccount>();               
                if (bankContext.Add(account))
                {
                    TempData["returnMessage"] = "success";
                }
                else
                {
                    TempData["returnMessage"] = "error";
                }
                return RedirectToAction("BankAccounts");
            }

            return View(bankAccountVM);
        }
        [Breadcrumb(AreaName = "Administrator", FromAction = "BankAccounts", Title = "Banka Hesabı Düzenleme Formu")]
        public IActionResult EditBankAccount(Guid id)
        {
            BankAccount account = bankContext.GetByID(id);
            if (account==null)
            {
                return NotFound();
            }

            BankAccountVM accountVM = account.Adapt<BankAccountVM>();

            return View(accountVM);
        }
        [HttpPost]
        public IActionResult EditBankAccount(BankAccountVM accountVM)
        {
            if (ModelState.IsValid)
            {
                BankAccount account = accountVM.Adapt<BankAccount>();
                if (bankContext.Update(account))
                {
                    TempData["returnMessage"] = "success";
                }
                else
                {
                    TempData["returnMessage"] = "error";
                }
                return RedirectToAction("BankAccounts");
            }

            return View(accountVM);
        }

        public IActionResult RemoveBankAccount(Guid id)
        {
            BankAccount account = bankContext.GetByID(id);
            if (account==null)
            {
                return NotFound();
            }

            if (bankContext.Remove(account))
            {
                TempData["returnMessage"] = "success";
            }
            else
            {
                TempData["returnMessage"] = "error";
            }

            return RedirectToAction("BankAccounts");
        }
        [Breadcrumb("Siparişler", AreaName = "Administrator")]
        public IActionResult Orders(int page=1, int pageSize=3)
        {
            IQueryable<Order> Orders = comOrder.GetAll().AsQueryable();
            IQueryable<CompanyPackage> packages = comPacks.GetAll().AsQueryable();
            //InnerJoin LinQ Başlangıç
            Orders = Orders.Join(packages, Order => Order.CompanyPackageId, CompanyPackage => CompanyPackage.Id, (Order, CompanyPackage) => Order).ToList().AsQueryable();
            //InnerJoin LinQ Bitiş

            return View(Orders.ToPagedList(page,pageSize));
        }

        [Breadcrumb(AreaName = "Administrator", FromAction = "Orders", Title = "Sipariş Detayları")]
        public IActionResult DetailsOrder(Guid id)
        {
            Order order = comOrder.GetByID(id);
            order.CompanyPackage = comPacks.GetByID(order.CompanyPackageId);
            order.Company = comContext.GetByID(order.CompanyId);

            return View(order);
        }

        [Breadcrumb(AreaName = "Administrator", FromAction = "Orders", Title = "Sipariş Düzenleme Formu")]
        public IActionResult EditOrder(Guid id)
        {
            Order order = comOrder.GetByID(id);
            return View(order.Adapt<OrderVM>());
        }

        [HttpPost]
        public IActionResult EditOrder(OrderVM ordervm)
        {
            Order order = comOrder.GetByID(Guid.Parse(ordervm.Id));
            if (order==null)
            {
                return NotFound();
            }
            order.Status = ordervm.Status;
            if(comOrder.Update(order))
            {
                TempData["returnMessage"] = "success";
                return RedirectToAction("Orders");
            }
            else
            {
                TempData["returnMessage"] = "error";
                ModelState.AddModelError("","Sipariş güncelenemedi. Hata!");
            }

            return View(ordervm);
        }


        public IActionResult ApproveOrder(Guid id)
        {
            // Bekleyen sipariş getirildi
            Order order = comOrder.GetByDefault(x=> x.Id==id && x.Status==CORE.Entity.Enums.Status.Pending);
            if (order == null)
            {
                return RedirectToAction("Orders",new { Hata="BekleyenSiparisBulunamadi" });
            }
            //Siparişin bağlı olduğu Paket getirildi
            CompanyPackage package = comPacks.GetByID(order.CompanyPackageId);
            // Etkilenecek olan Firma getirildi
            Company company = comContext.GetByID(order.CompanyId);

            order.Status = CORE.Entity.Enums.Status.Actived; // Sipariş durumu onaylandı
            company.ExpiresDate = company.ExpiresDate.AddMonths(package.AddMonth); // Firmanın bitiş süresine pakette bulunan ay süresi eklendi.
            company.UpdatedDate = DateTime.Now;



            if (comOrder.Update(order) && comContext.Update(company))
            {
                TempData["returnMessage"] = "success";
            }
            else
            {
                TempData["returnMessage"] = "error";
            }


            return RedirectToAction("Orders");
        }

        public IActionResult RemoveOrder(Guid id)
        {
            Order order = comOrder.GetByID(id);
            if (order == null)
            {
                return RedirectToAction("Orders");
            }

            if (comOrder.Remove(order))
            {
                TempData["returnMessage"] = "success";
            }
            else
            {
                TempData["returnMessage"] = "error";
            }

            return RedirectToAction("Orders");

        }

        public IActionResult ContactForm(int page = 1, int pageSize = 3)
        {
            List<Contact> contacts = contactContext.GetDefault(x => x.Status != CORE.Entity.Enums.Status.Deleted);

            return View(contacts.AsQueryable().ToPagedList(page, pageSize));
        }

        public IActionResult ContactDetail(Guid id)
        {
            Contact contact = contactContext.GetByID(id);
            return View(contact);
        }

        public IActionResult EditContact(Guid id)
        {
            Contact contact = contactContext.GetByID(id);
            return View(contact);
        }

        [HttpPost]
        public IActionResult EditContact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                if (contactContext.Update(contact))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("ContactForm");
                }
                else
                {
                    TempData["returnMessage"] = "error";
                }
            }

            return View(contact);

        }

        public IActionResult RemoveContact(Guid id)
        {
            Contact contact = contactContext.GetByID(id);

            if (contact==null)
            {
                return NotFound();
            }
            else
            {
                if (contactContext.Remove(contact))
                {
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("ContactForm");
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    return RedirectToAction("ContactForm");
                }
            }

            return RedirectToAction("ContactForm");

        }


    }
}
