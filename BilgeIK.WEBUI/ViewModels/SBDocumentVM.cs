using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewModels
{
    public class SBDocumentVM
    {
        [MaxLength(400)]
        [Display(Name = "Dosya, Fotoğraf")]
        [Required(ErrorMessage ="Dosya seçmelisiniz.")]
        public string File { get; set; }
        [MaxLength(1000)]
        [Required(ErrorMessage ="Açıklama Gereklidir.")]
        [Display(Name ="Açıklama")]
        public string Description { get; set; }
        public Guid SalaryBonusId { get; set; }

    }
}
