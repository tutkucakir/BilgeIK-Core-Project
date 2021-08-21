using BilgeIK.CORE.Entity.Concrete;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BilgeIK.MODEL.Entities
{
    public class CompanyPackage:CoreEntity
    {
        [MaxLength(400)]
        public string Name { get; set; }
        [MaxLength(400)]
        public string IconCode { get; set; }
        public int AddMonth { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ExpiresDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal?  NewPrice { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
