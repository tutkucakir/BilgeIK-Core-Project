using BilgeIK.MODEL.Entities;
using BilgeIK.SERVICE.TwoFactorService;
using BilgeIK.MODEL.Context;
using BilgeIK.WEBUI.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BilgeIK.CORE.Entity.Enums;
using BilgeIK.CORE.Service;
using BilgeIK.SERVICE.Helper;
using Microsoft.AspNetCore.Authorization;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using SmartBreadcrumbs.Attributes;

namespace BilgeIK.WEBUI.Controllers
{
    public class AuthController : BaseController
    {
        protected readonly ICoreService<Company> _company;
        private readonly TwoFactorService _twoFactorService;
        private readonly EmailSender _emailSender;
        private IHostingEnvironment _hostingEnv;
        public AuthController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TwoFactorService twoFactorService, EmailSender emailSender, ICoreService<Company> company, IHostingEnvironment hostingEnv) : base(logger, userManager, signInManager)
        {
            _company = company;
            _twoFactorService = twoFactorService;
            _emailSender = emailSender;
            _hostingEnv = hostingEnv;
        }

        public IActionResult SignUp()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignupVM signUpVM)
        {
            if (ModelState.IsValid)
            {

                if (userManager.Users.Any(u => u.PhoneNumber == signUpVM.CompanyPhone))
                {
                    ModelState.AddModelError("", "Bu telefon numarasını başka bir üye kullanmaktadır.");
                    return View(signUpVM);
                }

                Company c1 = new Company();
                c1.CompanyName = signUpVM.CompanyName;
                c1.CompanyPhone = signUpVM.CompanyPhone;
                c1.CompanyAdress = signUpVM.CompanyAdress;
                c1.RegisteredDate = DateTime.Now;
                c1.UpdatedDate = DateTime.Now;
                c1.ExpiresDate = DateTime.Now.AddMonths(3);
                _company.Add(c1);

                AppUser user = new AppUser();
                user.UserName = signUpVM.UserName;
                user.Email = signUpVM.Email;
                user.PhoneNumber = signUpVM.CompanyPhone;
                user.FirstName = signUpVM.FirstName;
                user.LastName = signUpVM.LastName;
                user.CompanyId = c1.Id;

                IdentityResult result = await userManager.CreateAsync(user, signUpVM.Password);

                if (result.Succeeded)
                {
                    IdentityResult result2=await userManager.AddToRoleAsync(user,"Şirket Temsilcisi");
                    if (!result2.Succeeded)
                    {
                        AddModelError(result2);
                    }
                    string confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    string link = Url.Action("ConfirmEmail", "Auth", new
                    {
                        userId = user.Id,
                        token = confirmationToken
                    }, protocol: HttpContext.Request.Scheme);
                    EmailConfirmation.SendEmail(link, user.Email);                    

                    return RedirectToAction("Index","Home");

                }
                else
                {
                    AddModelError(result);
                }

            }

            return View(signUpVM);
        }

        public IActionResult LogIn(string ReturnUrl="/")
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginVM userLogin)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(userLogin.Email);

                if (user != null)
                {
                    if (await userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("", "Hesabınız bir süreliğine kilitlenmiştir. Sonra tekrar deneyin. Açılış süresi: " + user.LockoutEnd.ToString());

                        return View(userLogin);
                    }

                    if (userManager.IsEmailConfirmedAsync(user).Result == false)
                    {
                        ModelState.AddModelError("", "Email adresiniz doğrulanmadığından oturum açılamıyor! Lütfen e-posta gelen kutunuzu kontrol ediniz.");
                        return View(userLogin);
                    }

                    if (user.Status!=Status.Actived)
                    {
                        ModelState.AddModelError("", "Hesabınız aktif olmadığından oturumunuzu açamıyoruz.");
                        return View(userLogin);
                    }

                    if (user.CompanyId!=null)
                    {
                        var companyID = user.CompanyId ?? new Guid();
                        Company company = _company.GetByID(companyID);
                        if (company!=null)
                        {
                            if (company.Status==Status.Deleted)
                            {
                                ModelState.AddModelError("", "Firma hesabınız silindiğinden oturumunuzu açamıyoruz.");
                                return View(userLogin);
                            }else if(company.Status != Status.Actived)
                            {
                                ModelState.AddModelError("", "Firma hesabınız aktif olmadığından oturumunuzu açamıyoruz. Yöneticilerimiz hesabınızı onayladıktan sonra giriş yapabilirsiniz. Hata olduğunu düşünüyorsanız iletişim formundan bizlere ulaşınız.");
                                return View(userLogin);
                            }
                        }

                    }


                    bool userCheck = await userManager.CheckPasswordAsync(user, userLogin.Password);

                    if (userCheck)
                    {
                        await userManager.ResetAccessFailedCountAsync(user);
                        await signInManager.SignOutAsync();
                        Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, userLogin.Password, userLogin.RememberMe, false);



                        if (result.RequiresTwoFactor)
                        {
                            //if (user.TwoFactor == (int)TwoFactor.Email || user.TwoFactor == (int)TwoFactor.Phone)
                            //{
                            //    HttpContext.Session.Remove("currentTime");
                            //}
                            return RedirectToAction("TwoFactorLogin", "Auth", new { ReturnUrl = TempData["ReturnUrl"].ToString() });
                        }
                        else
                        {
                            if (TempData["ReturnUrl"].ToString() !="/")
                            {
                                return Redirect(TempData["ReturnUrl"].ToString());
                            }

                            if (userManager.IsInRoleAsync(user,"Site Yöneticisi").Result)
                            {
                                return RedirectToAction("Index","Dashboard",new { area ="Administrator"});
                            }else if(userManager.IsInRoleAsync(user, "Şirket Temsilcisi").Result)
                            {
                                return RedirectToAction("Index", "ManagerDashboard", new { area = "Manager" });
                            }
                            else
                            {
                                return RedirectToAction("Index", "PersonalDashboard", new { area="Personal"});
                            }
                            
                        }


                    }
                    else
                    {
                        await userManager.AccessFailedAsync(user); // Yanlış giriş sayısını 1 artıracaktır.

                        int fail = await userManager.GetAccessFailedCountAsync(user);

                        ModelState.AddModelError("", $" {fail} kez parolanızı yanlış girdiniz... ");

                        if (fail == 3)
                        {
                            await userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(20)));
                            ModelState.AddModelError("", "Hesabınız 3 başarısız girişten dolayı 20 dakika süre ile kilitlenmiştir. Lütfen sonra tekrar deneyiniz.");
                        }
                        else
                        {
                            ModelState.AddModelError("", "E-Posta adresiniz veya parolanız yanlış girilmiş...");
                        }

                    }


                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı...");
                }



            }



            return View(userLogin);
        }


        [Authorize]
        public ActionResult LogOut()
        {
            signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [Breadcrumb(Title ="Profil Ayarları", FromController = typeof(HomeController), FromAction ="Index")]
        [Authorize]
        public async Task<ActionResult> ProfileSettings()
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);

            if (await userManager.IsInRoleAsync(user, "Çalışan"))
            {
                ViewBag.SelectedRole = "0";
            }
            else if (await userManager.IsInRoleAsync(user, "Şirket Temsilcisi"))
            {
                ViewBag.SelectedRole = "1";
            }
            else if(await userManager.IsInRoleAsync(user, "Site Yöneticisi"))
            {
                ViewBag.SelectedRole = "2";
            }

            return View(user.Adapt<UserVM>());
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> ProfileSettings(UserVM userVM, List<IFormFile> Photo)
        {
            AppUser user = await userManager.FindByIdAsync(userVM.Id);

            if (user==null)
            {
                return NotFound();
            }


            user.FirstName = userVM.FirstName;
            user.LastName = userVM.LastName;
            user.BirthDate = userVM.BirthDate;
            user.Adress = userVM.Adress;
            user.Gender = userVM.Gender;

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

            string imgPath = Upload.ImageUpload(Photo, _hostingEnv, out bool imgResult);
            if (imgResult)
            {
                user.Photo = imgPath;
            }


            if (ModelState.IsValid)
            {
                await userManager.UpdateAsync(user);
                TempData["returnMessage"] = "success";
                return RedirectToAction("ProfileSettings");
            }

            return View(userVM);
        }



        public IActionResult ResetPassword()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }


        [HttpPost]
        public IActionResult ResetPassword(PasswordResetViewModel passwordResetViewModel)
        {
            AppUser user = userManager.FindByEmailAsync(passwordResetViewModel.Email).Result;
            if (user != null)
            {
                string passwordResetToken = userManager.GeneratePasswordResetTokenAsync(user).Result;

                string passwordResetLink = Url.Action("ResetPasswordConfirm", "Auth", new
                {
                    userId = user.Id,
                    token = passwordResetToken
                }, HttpContext.Request.Scheme);

                PasswordReset.PasswordResetSendEmail(passwordResetLink, user.Email);
                ViewBag.status = "successfull";

            }
            else
            {
                ModelState.AddModelError("", "Girdiğiniz e-posta için kullanıcı bulunamadı!");
            }

            return View(passwordResetViewModel);
        }



        public IActionResult ResetPasswordConfirm(string userId, string token)
        {


            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["userId"] = userId;
                TempData["token"] = token;
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm([Bind("PasswordNew")] PasswordResetViewModel passwordResetViewModel)
        {
            string token = "";
            string userId = "";
            if (TempData["token"] == null || TempData["userId"] == null)
            {
                ModelState.AddModelError("", "Parola sıfırlama linki geçerliliğini yitirdi! E-postanızı kontrol ediniz yada aynı link ile tekrar parola sıfırlama ekranına erişiniz.");
                ViewBag.status = "errorToken";
            }
            else
            {
                token = TempData["token"].ToString();
                userId = TempData["userId"].ToString();
            }


            AppUser user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                IdentityResult result = await userManager.ResetPasswordAsync(user, token, passwordResetViewModel.PasswordNew);

                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);

                    ViewBag.status = "success";
                }
                else
                {
                    AddModelError(result);
                }


            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı!");
            }

            return View();

        }


        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);

            IdentityResult result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                ViewBag.status = "Email adresiniz onaylanmıştır. Login ekranından giriş yapabilirsiniz";
                user.Status = Status.Actived;
            }
            else
            {
                ViewBag.status = "Bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz.";
            }

            return View();
        }

        public IActionResult EmailConfirmRequest()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailConfirmRequest(EmailConfirmVM emailConfirm)
        {
            if (ModelState.IsValid)
            {
                if (userManager.Users.Any(u => u.Email == emailConfirm.Email && u.EmailConfirmed == false))
                {
                    AppUser user = userManager.Users.FirstOrDefault(u => u.Email == emailConfirm.Email && u.EmailConfirmed == false && u.Status!=Status.Deleted && u.Status != Status.DeActived);
                    string confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    string link = Url.Action("ConfirmEmail", "Auth", new
                    {
                        userId = user.Id,
                        token = confirmationToken
                    }, protocol: HttpContext.Request.Scheme);
                    EmailConfirmation.SendEmail(link, user.Email);
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Bu e-posta hesabı için doğrulama bekleyen hesap bulunamadı.");
                    return View(emailConfirm);
                }
            }

            return View(emailConfirm);
        }
        [Breadcrumb(Title ="Parola Değiştirme Formu", FromAction ="ProfileSettings")]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(PassChangeVM passChangeVM)
        {

            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);

            if (user==null)
            {
                return RedirectToAction("AccessDenied");
            }
            else
            {

                IdentityResult result = await userManager.ChangePasswordAsync(user, passChangeVM.Password, passChangeVM.PasswordNew);

                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);
                    TempData["returnMessage"] = "success";
                    return RedirectToAction("ProfileSettings");
                }
                else
                {
                    TempData["returnMessage"] = "error";
                    AddModelError(result);
                }
            }


            return View(passChangeVM);
        }
        [Authorize]
        [Breadcrumb(Title = "Hesap Kapatma Formu", FromAction = "ProfileSettings")]
        public IActionResult DeActiveAccount()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeActiveAccount(DeactivateVM deactivateVM)
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);

            if (user==null)
            {
                return RedirectToAction("AccessDenied");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    user.Status = Status.DeActived;
                    await userManager.UpdateAsync(user);
                    return RedirectToAction("LogOut");
                }
            }


            return View();
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            ViewBag.message = "Bu sayfaya erişim izniniz bulunmamaktadır. Web Sitesinde bir hata olduğunu düşünüyorsanız Site Yöneticisi ile iletişim kurunuz.";
            return View();
        }


    }
}
