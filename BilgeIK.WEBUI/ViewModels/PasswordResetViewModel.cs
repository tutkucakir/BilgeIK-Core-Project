using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewModels
{
    public class PasswordResetViewModel
    {
        [Display(Name = "E-Posta")]
        [Required(ErrorMessage = "E-Posta alanı gereklidir.")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage = "Parola girilmesi gereklidir.")]
        [Display(Name = "Yeni Parolanız")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Parolanız en az 4 karakter uzunluğunda olmalıdır.")]
        public string PasswordNew { get; set; }
    }
}
