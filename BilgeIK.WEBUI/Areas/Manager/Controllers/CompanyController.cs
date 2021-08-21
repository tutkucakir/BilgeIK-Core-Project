using BilgeIK.CORE.Service;
using BilgeIK.MODEL.Entities;
using BilgeIK.WEBUI.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.Areas.Manager.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly ICoreService<CompanyPackage> comPacks;
        private readonly ICoreService<Company> comContext;
        private readonly ICoreService<Order> comOrder;
        private readonly ICoreService<BankAccount> bankContext;
        public CompanyController(ILogger<ManagerDashboardController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, ICoreService<CompanyPackage> comPacks, ICoreService<Company> comContext, ICoreService<Order> comOrder, ICoreService<BankAccount> bankContext) : base(logger, userManager, signInManager, roleManager)
        {
            this.comPacks = comPacks;
            this.comContext = comContext;
            this.comOrder = comOrder;
            this.bankContext = bankContext;
        }

        [Breadcrumb(Title = "Firma Paketleri", AreaName = "Manager", FromController = typeof(ManagerDashboardController))]
        public IActionResult CompanyPackages()
        {
            List<CompanyPackage> packages= comPacks.GetDefault(x => x.Status == CORE.Entity.Enums.Status.Actived && x.ExpiresDate > DateTime.Now);
            return View(packages);
        }

        [Breadcrumb(Title = "Paket Sipariş Ekranı", AreaName = "Manager", FromAction = "CompanyPackages")]
        public async Task<IActionResult> PackageOrder(Guid id)
        {
            AppUser manager = await userManager.FindByNameAsync(User.Identity.Name);
            Guid companyGuid = manager.CompanyId ?? Guid.NewGuid();
            Company company = comContext.GetByID(companyGuid);
            CompanyPackage package = comPacks.GetByID(id);

            if (manager==null || company==null || package==null)
            {
                return NotFound();
            }

            return View(Tuple.Create<Company,CompanyPackage>(company,package));
        }

        [HttpPost]
        public async Task<IActionResult> PackageOrder(string id)
        {
            if(id==null)
            {
                return NotFound();
            }
            AppUser manager = await userManager.FindByNameAsync(User.Identity.Name);
            Guid companyGuid = manager.CompanyId ?? Guid.NewGuid();
            Company company = comContext.GetByID(companyGuid);
            CompanyPackage package = comPacks.GetByID(Guid.Parse(id));
            if (manager == null || company == null || package == null)
            {
                return NotFound();
            }
            Order order = new Order();
            order.Company = company;
            Guid companyId = manager.CompanyId ?? new();
            order.CompanyId = companyId;
            order.CompanyPackage = package;
            order.CompanyPackageId = package.Id;
            order.OrderDate = DateTime.Now;
            order.Status = CORE.Entity.Enums.Status.Pending;
            if (comOrder.GetAll().Count==0)
            {
                order.OrderNo = 1000;
            }
            else
            {
                Order sonOrder = comOrder.GetAll().OrderByDescending(x => x.OrderNo).FirstOrDefault();
                order.OrderNo=sonOrder.OrderNo + 1;
            }

            if (package.NewPrice!=null)
            {
                decimal fiyat = package.NewPrice ?? new();
                order.TotalPrice = fiyat;
            }
            else
            {
                order.TotalPrice = package.Price;
            }

            if (comOrder.Add(order))
            {
                TempData["returnMessage"] = "success";
                return RedirectToAction("OrderDetails","Company", new { Id=order.Id});
            }
            else
            {
                TempData["returnMessage"] = "error";
            }

            return View();
        }
        [Breadcrumb(Title = "Sipariş Detayları", AreaName = "Manager", FromAction = "PackageOrder")]
        public async Task<IActionResult> OrderDetails(Guid id)
        {
            AppUser manager = await userManager.FindByNameAsync(User.Identity.Name);
            Order order = comOrder.GetByDefault(x=>x.Id==id && x.CompanyId==manager.CompanyId);
            if (order==null)
            {
                return NotFound();
            }

            order.CompanyPackage = comPacks.GetByID(order.CompanyPackageId);
            order.Company = comContext.GetByID(order.CompanyId);

            List<BankAccount> accounts = bankContext.GetActive();

            return View(Tuple.Create<Order,List<BankAccount>>(order,accounts));
        }




    }
}
