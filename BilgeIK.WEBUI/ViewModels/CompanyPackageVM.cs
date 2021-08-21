using BilgeIK.CORE.Entity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewModels
{
    public class CompanyPackageVM
    {
        [Display(Name="Paket Adı")]
        [Required(ErrorMessage ="Paket Adı zorunlu alandır.")]
        public string Name { get; set; }
        [Display(Name = "Icon Kodu (Örnek: fa fa-users) ")]
        [Required(ErrorMessage = "İkon kodu alanı zorunlu alandır.")]
        [MaxLength(400)]
        public string IconCode { get; set; }

        [Display(Name="Paket Süresi/Ay")]
        [Required(ErrorMessage ="Paket süresi zorunlu alandır.")]
        public int AddMonth { get; set; }

        [Display(Name="Oluşturulma Tarihi")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        [Display(Name="Kampanya Bitiş Tarihi")]
        [DataType(DataType.Date)]
        public DateTime ExpiresDate { get; set; }

        [Display(Name="Fiyat")]
        [Required(ErrorMessage = "Ücret bilgisini girmelisiniz.")]
        [RegularExpression(@"^(\d+),(\d{2})$", ErrorMessage = "Tutar 0,00 - 9999999999,99 aradığında olmalıdır. ")]
        public string Price { get; set; }

        [Display(Name = "Kampanyalı Fiyatı")]
        [Required(ErrorMessage = "Ücret bilgisini girmelisiniz.")]
        [RegularExpression(@"^(\d+),(\d{2})$", ErrorMessage = "Tutar 0,00 - 9999999999,99 aradığında olmalıdır. ")]
        public string NewPrice { get; set; }
        [Display(Name="Açıklama")]
        public string Description { get; set; }
        public string Id { get; set; }
        [Display(Name = "Durumu")]
        public Status Status { get; set; }
    }
}
