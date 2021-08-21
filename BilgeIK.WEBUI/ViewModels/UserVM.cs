using BilgeIK.CORE.Entity.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewModels
{
    public class UserVM
    {
        [MaxLength(11)]
        [Display(Name ="T.C. Kimlik No")]
        [Required(ErrorMessage ="T.C. Kimlik No zorunlu alandır.")]
        public string NationalIDCardNumber { get; set; }
        [MaxLength(1000)]
        [Display(Name = "İş Ünvanı/Görevi")]
        public string JobTitle { get; set; }

        [Display(Name ="Doğum Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [MaxLength(60)]
        [Display(Name ="Adı")]
        [Required(ErrorMessage = "Adı zorunlu alandır.")]
        public string FirstName { get; set; }

        [MaxLength(60)]
        [Display(Name = "Soyadı")]
        [Required(ErrorMessage = "Soyadı zorunlu alandır.")]
        public string LastName { get; set; }

        [MaxLength(400)]
        [Display(Name = "Fotoğraf")]
        public string Photo { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Adres")]
        public string Adress { get; set; }

        [Display(Name = "Cinsiyet")]
        public Gender Gender { get; set; }

        [Display(Name = "Durumu")]
        public Status Status { get; set; }

        [Display(Name = "Hesap Kilidi Sonlanma Zamanı")]
        public DateTimeOffset? LockoutEnd { get; set; }

        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; }

        [Display(Name = "E-Posta Doğrulaması")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "E-Posta")]
        [Required(ErrorMessage = "E-Posta alanı gereklidir.")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage ="Kullanıcı Adı alanı gereklidir.")]
        public string UserName { get; set; }
        public string Id { get; set; }
        [Display(Name = "Kilitleme Etkin mi?")]
        public bool LockoutEnabled { get; set; }
        [Display(Name = "Hatalı Giriş Denemesi")]
        public int AccessFailedCount { get; set; }

    }
}
