using BilgeIK.CORE.Entity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewModels
{
    public class CompanyVM
    {
        [Display(Name ="Şirket Adı")]
        [Required(ErrorMessage ="Şirket Adı zorunlu alandır.")]
        public string CompanyName { get; set; }
        [Display(Name ="Şirket Telefon Numarası")]
        [Required(ErrorMessage="Şirket telefon numarası zorunlu alandır.")]
        [RegularExpression(@"^(0(\d{3}) (\d{3}) (\d{2}) (\d{2}))$", ErrorMessage = "Telefon numarası uygun formatta girilmemiş...")]
        public string CompanyPhone { get; set; }
        [Display(Name ="Şirket Adresi")]
        public string CompanyAdress { get; set; }
        [Display(Name ="Şirket Logosu")]
        public string Photo { get; set; }
        [Display(Name ="Kayıt Tarihi")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Kayıt tarihi zorunlu alandır.")]
        public DateTime RegisteredDate { get; set; }
        [Display(Name = "Yenileme Tarihi")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Yenileme tarihi zorunlu alandır.")]
        public DateTime UpdatedDate { get; set; }
        [Display(Name = "Üyelik Biriş Tarihi")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Üyelik bitiş tarihi zorunlu alandır.")]
        public DateTime ExpiresDate { get; set; }
        [Display(Name = "Onay Durumu")]
        public Status Status { get; set; }
        public string Id { get; set; }
    }
}
