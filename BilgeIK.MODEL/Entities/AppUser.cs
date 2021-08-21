using BilgeIK.CORE.Entity.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BilgeIK.MODEL.Entities
{
    public class AppUser : IdentityUser
    {
        [MaxLength(11)]
        public string NationalIDCardNumber { get; set; }
        [MaxLength(1000)]
        public string JobTitle { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        [MaxLength(60)]
        public string FirstName { get; set; }
        [MaxLength(60)]
        public string LastName { get; set; }
        [MaxLength(400)]
        public string Photo { get; set; }
        [MaxLength(1000)]
        public string Adress { get; set; }
        public DateTime? DeactivationTime { get; set; }
        [MaxLength(1000)]
        public string DeactivationReason { get; set; }
        public Gender Gender { get; set; }
        public Status Status { get; set; }
        public Guid? CompanyId { get; set; }
        public Company Company { get; set; }
        public virtual List<Leave> Leaves { get; set; }
        public virtual List<UserDocument> UserDocuments { get; set; }
        public virtual List<Report> Reports { get; set; }
        public virtual List<SalaryBonus> SalaryBonus { get; set; }
        public virtual List<CustomerComment> CustomerComment { get; set; }
    }
}
