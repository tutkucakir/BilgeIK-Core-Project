using BilgeIK.MODEL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BilgeIK.MODEL.Context
{
    public class  AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }


        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyPackage> CompanyPackages { get; set; }
        public DbSet<Definition> Definitions { get; set; }
        public DbSet<CustomerComment> CustomerComments { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<SalaryBonus> SalaryBonus { get; set; }
        public DbSet<UserDocument> UserDocuments { get; set; }
        public DbSet<SBDocument> SBDocuments { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Contact> Contacts { get; set; }

    }
}
