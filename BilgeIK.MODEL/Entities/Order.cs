using BilgeIK.CORE.Entity.Concrete;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BilgeIK.MODEL.Entities
{
    public class Order : CoreEntity
    {

        [DataType(DataType.Date)]
        [Display(Name = "Sipariş Tarihi")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Fiyat")]
        [Column(TypeName = "decimal(18,2)")]
        public Decimal TotalPrice { get; set; }
        public Guid CompanyPackageId { get; set; }
        public CompanyPackage CompanyPackage { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        [Display(Name = "Sipariş No")]
        public int OrderNo { get; set; }
    }
}
