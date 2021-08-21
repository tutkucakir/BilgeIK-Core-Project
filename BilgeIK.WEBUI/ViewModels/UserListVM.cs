using BilgeIK.CORE.Entity.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewModels
{
    public class UserListVM
    {
        [Display(Name = "Email")]
        public string UserEmail { get; set; }
        [Display(Name="Kullanıcı Adı")]
        public string UserName { get; set; }
        [Display(Name ="Personelin Görevi")]
        public string JobTitle { get; set; }
        [Display(Name = "Fotoğraf")]
        public string Photo { get; set; }
        [Display(Name = "Adı")]
        public string FirstName { get; set; }
        [Display(Name = "Soyadı")]
        public string LastName { get; set; }

        [Display(Name ="Firma Adı")]
        public string CompanyName { get; set; }
        [Display(Name = "E-Posta Doğrulaması")]
        public bool EmailConfirmed { get; set; }

        [Display(Name ="Yetkiler")]
        public IList<string> Roles { get; set; }
        public Status Status { get; set; }
        public string Id { get; set; }
    }
}
