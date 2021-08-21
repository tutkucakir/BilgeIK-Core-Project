using BilgeIK.CORE.Entity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewModels
{
    public class BankAccountVM
    {
        [Display(Name ="IBAN Numarası")]
        [Required(ErrorMessage ="IBAN numarası zorunlu alandır.")]
        [MaxLength(400)]
        public string IBAN { get; set; }

        [Display(Name = "Banka Adı")]
        [Required(ErrorMessage = "Banka Adı zorunlu alandır.")]
        public string Bank { get; set; }

        [Display(Name = "Banka Şubesi")]
        [Required(ErrorMessage = "Banka Şubesi zorunlu alandır.")]
        [MaxLength(400)]
        public string BankBranch { get; set; }
        [Display(Name = "Hesap Numarası")]
        [Required(ErrorMessage = "Hesap numarası zorunlu alandır.")]
        public int AccountNumber { get; set; }

        [Display(Name = "Hesap Sahibi Adı")]
        [Required(ErrorMessage = "Hesap Sahibi Adı zorunlu alandır.")]
        [MaxLength(500)]
        public string AccountHolderInfo { get; set; }
        public Guid Id { get; set; }
        public Status Status { get; set; }
    }
}
