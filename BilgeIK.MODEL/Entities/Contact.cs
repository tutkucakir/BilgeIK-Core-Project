using BilgeIK.CORE.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeIK.MODEL.Entities
{
    public class Contact : CoreEntity
    {
        [MaxLength(255)]
        [Display(Name ="Adı ve Soyadı")]
        public string Name { get; set; }
        [MaxLength(255)]
        [Display(Name = "E-Posta Adresi")]
        [EmailAddress]
        public string Mail { get; set; }
        [MaxLength(1000)]
        [Display(Name = "Mesajı")]
        public string Message { get; set; }
        [Display(Name = "Gönderilme Tarihi")]
        public DateTime? PostedDate { get; set; }
    }
}
