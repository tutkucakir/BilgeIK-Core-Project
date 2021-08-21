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
    public class OrderVM
    {
        [Display(Name = "Sipariş No")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OrderNo { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Sipariş Tarihi")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Fiyat")]
        [Required(ErrorMessage = "Ücret bilgisini girmelisiniz.")]
        [RegularExpression(@"^(\d+),(\d{2})$", ErrorMessage = "Tutar 0,00 - 9999999999,99 aradığında olmalıdır. ")]
        public string TotalPrice { get; set; }
        public Guid CompanyPackageId { get; set; }
        public CompanyPackage CompanyPackage { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public string Id { get; set; }
        [Display(Name = "Durum")]
        public Status Status { get; set; }
    }
}
