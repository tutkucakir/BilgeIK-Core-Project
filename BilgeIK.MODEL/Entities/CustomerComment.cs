using BilgeIK.CORE.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeIK.MODEL.Entities
{
    public class CustomerComment : CoreEntity
    {
        [MaxLength(1000)]
        [Display(Name ="Yorumunuz")]
        public string Comment { get; set; }
        public bool FeaturedComment { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
