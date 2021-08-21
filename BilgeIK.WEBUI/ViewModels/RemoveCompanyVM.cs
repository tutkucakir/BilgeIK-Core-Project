using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewModels
{
    public class RemoveCompanyVM
    {
        [Display(Name ="Firma Kesabı Kapatma Sebebi")]
        [Required(ErrorMessage ="Bir kapatma sebebi girmelisiniz...")]
        [MaxLength(1000)]
        public string DeactivationReason { get; set; }
    }
}
