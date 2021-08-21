using BilgeIK.CORE.Entity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewModels
{
    public class UserDocVM
    {
        [Required(ErrorMessage ="Dosya isim alanı gereklidir")]
        [MaxLength(400)]
        [Display(Name="Dosya Adı")]
        public string Name { get; set; }
        [MaxLength(400)]
        [Display(Name="Dosya Seçin")]
        public string FileUrl { get; set; }
        [Display(Name = "Durumu")]
        public Status Status { get; set; }
        public string Id { get; set; }
    }
}
