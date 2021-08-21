using BilgeIK.CORE.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BilgeIK.MODEL.Entities
{
    public class Company : CoreEntity
    {
        [MaxLength(400)]
        public string CompanyName { get; set; }
        [MaxLength(400)]
        public string CompanyPhone { get; set; }
        [MaxLength(1000)]
        public string CompanyAdress { get; set; }
        [MaxLength(400)]
        public string Photo { get; set; }
        [DataType(DataType.Date)]
        public DateTime RegisteredDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime UpdatedDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ExpiresDate { get; set; }
        public DateTime? DeactivationTime { get; set; }
        [MaxLength(1000)]
        public string DeactivationReason { get; set; }
        public virtual List<AppUser> AppUsers { get; set; }

    }
}
