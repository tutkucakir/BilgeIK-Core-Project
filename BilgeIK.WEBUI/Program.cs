using BilgeIK.MODEL.Context;
using BilgeIK.MODEL.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var identityDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            identityDbContext.Database.Migrate();

            if (!userManager.Users.Any())
            {
                var newRole = new AppRole() {  Name="Çalýþan"};
                roleManager.CreateAsync(newRole).Wait();
                var newRole1 = new AppRole() {  Name="Þirket Temsilcisi"};
                roleManager.CreateAsync(newRole1).Wait();
                var newRole2 = new AppRole() {  Name="Site Yöneticisi"};
                roleManager.CreateAsync(newRole2).Wait();

                identityDbContext.SaveChanges();

                var newUser = new AppUser() { UserName="Tutku", Email="tutku@tutkucakir.com.tr", BirthDate=DateTime.Parse("1991-06-30") , EmailConfirmed=true,  };
                userManager.CreateAsync(newUser, "Password12*").Wait();

                userManager.AddToRoleAsync(newUser, roleManager.FindByIdAsync(newRole2.Id).Result.Name).Wait();
                identityDbContext.SaveChanges();

            }


            host.Run();

        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
