using BilgeIK.CORE.Service;
using BilgeIK.MODEL.Context;
using BilgeIK.MODEL.Entities;
using BilgeIK.SERVICE.Base;
using BilgeIK.SERVICE.IdentityCustomValidation;
using BilgeIK.SERVICE.TwoFactorService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartBreadcrumbs.Extensions;
using System;


namespace BilgeIK.WEBUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBreadcrumbs(GetType().Assembly, options =>
            {
                // Testing
                options.DontLookForDefaultNode = false;
                options.TagName = "nav";
                options.TagClasses = "ul-breadcrumb";
                options.OlClasses = "ul-breadcrumb-items";
                options.LiClasses = "breadcrumb-item";
                options.ActiveLiClasses = "breadcrumb-item active";
                options.AriaLabel = "breadcrumb";
            });

            services.Configure<TwoFactorOptions>(Configuration.GetSection("TwoFactorOptions"));
            services.AddScoped<TwoFactorService>();
            services.AddScoped<EmailSender>();
            services.AddScoped<SmsSender>();
            services.AddScoped(typeof(ICoreService<>), typeof(BaseService<>));
            services.AddDbContext<AppDbContext>(opts =>
            {
                opts.UseSqlServer(Configuration["ConnectionStrings:DefaultConnectionString"]);
            });

            services.AddAuthentication();
            services.AddIdentity<AppUser, AppRole>(opts =>
             {

                 opts.User.RequireUniqueEmail = true;
                 opts.User.AllowedUserNameCharacters = "abcçdefgðhýijklmnoöpqrsþtuüvwxyzABCÇDEFGÐHÝIJKLMNÖOPQRSÞTUÜVWXYZ0123456789-._@+";

                 opts.Password.RequiredLength = 4;
                 opts.Password.RequireNonAlphanumeric = false;
                 opts.Password.RequireLowercase = false;
                 opts.Password.RequireUppercase = false;
                 opts.Password.RequireDigit = false;

             }).AddPasswordValidator<CustomPasswordValidator>().AddUserValidator<CustomUserValidator>().AddErrorDescriber<CustomIdentityErrorDescriber>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            CookieBuilder cookieBuilder = new CookieBuilder();
            cookieBuilder.Name = "BilgeIK";
            cookieBuilder.HttpOnly = false;
            cookieBuilder.SameSite = SameSiteMode.Lax;
            cookieBuilder.SecurePolicy = CookieSecurePolicy.Always; // Always https, none ayar yapýlmaz, sameasrequest https or http

            services.ConfigureApplicationCookie(opts =>
            {
                opts.LoginPath = new PathString("/Auth/LogIn");
                opts.LogoutPath = new PathString("/Auth/LogOut");
                opts.Cookie = cookieBuilder;
                opts.SlidingExpiration = true;
                opts.ExpireTimeSpan = TimeSpan.FromDays(30);
                opts.AccessDeniedPath = new PathString("/Auth/AccessDenied");
            });

            //services.AddScoped<IClaimsTransformation, ClaimProvider.ClaimProvider>();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.Name = "MainSession";
            });

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Administrator",
                    areaName: "Administrator",
                    pattern: "Administrator/{controller=Dashboard}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(
                    name: "Manager",
                    areaName: "Manager",
                    pattern: "Manager/{controller=ManagerDashboard}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(
                    name: "Personal",
                    areaName: "Personal",
                    pattern: "Personal/{controller=PersonalDashboard}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
