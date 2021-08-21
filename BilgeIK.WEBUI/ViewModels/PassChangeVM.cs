using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewModels
{
    public class PassChangeVM
    {
        [Required(ErrorMessage = "Mevcut parolanızın girilmesi gereklidir.")]
        [Display(Name = "Mevcut Parolanız")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Yeni Parola girilmesi gereklidir.")]
        [Display(Name = "Yeni Parolanız")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Parolanız en az 4 karakter uzunluğunda olmalıdır.")]
        public string PasswordNew { get; set; }

        [Required(ErrorMessage = "Yeni Parolanızın tekrar girilmesi gereklidir.")]
        [Display(Name = "Yeni Parolanız (Doğrulayınız)")]
        [DataType(DataType.Password)]
        [Compare("PasswordNew", ErrorMessage ="Parola alanları aynı olmalıdır.")]
        public string PasswordNew2 { get; set; }

    }
}
