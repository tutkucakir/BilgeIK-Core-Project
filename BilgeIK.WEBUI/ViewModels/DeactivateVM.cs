using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewModels
{
    public class DeactivateVM
    {
        [Display(Name = "Devre Dışı Bırakma Sebebi")]
        [Required(ErrorMessage = "Hesabınızı devre dışı bırakmadan önce ufak bir açıklama girmenizi istiyoruz.")]
        [MaxLength(1000)]
        public string DeactivationReason { get; set; }
    }
}
