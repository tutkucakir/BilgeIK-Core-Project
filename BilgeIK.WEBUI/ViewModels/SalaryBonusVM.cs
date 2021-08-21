using BilgeIK.CORE.Entity.Enums;
using BilgeIK.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewModels
{
    public class SalaryBonusVM
    {
        [Display(Name="Ek Ücret")]
        [Required(ErrorMessage ="Ücret bilgisini girmelisiniz.")]
        [RegularExpression(@"^(\d+),(\d{2})$", ErrorMessage = "Tutar 0,00 - 9999999999,99 aradığında olmalıdır. Tamsayı ardında virgül ve 2 sayı gelmelidir. ")]
        public string Bonus { get; set; }
        [Display(Name ="Açıklama")]
        public string Description { get; set; }
        [Display(Name ="Onay Tarihi")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Display(Name ="Ödeme Alan")]
        public Guid? UserId { get; set; }
        public AppUser AppUser { get; set; }

        [Display(Name = "Ödemeyi Sisteme Giren/Onaylayan")]
        public Guid? ApproverUserId { get; set; }
        public AppUser ApproverUser { get; set; }

        [Display(Name = "Durum")]
        public Status Status { get; set; }
        public string Id { get; set; }


    }
}
