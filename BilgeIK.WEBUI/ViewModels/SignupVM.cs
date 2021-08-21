using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewModels
{
    public class SignupVM
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şirket E-Posta adresi gereklidir.")]
        [Display(Name = "Şirket E-Posta Adresi")]
        [EmailAddress(ErrorMessage = "E-Posta adresiniz doğru formatta girilmemiştir.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola gereklidir.")]
        [Display(Name = "Parola")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Adınız gereklidir.")]
        [Display(Name = "Adınız")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyadınız gereklidir.")]
        [Display(Name = "Soyadınız")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Şirket Adı gereklidir.")]
        [Display(Name = "Şirket Adı")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Şirket Adı gereklidir.")]
        [Display(Name = "Şirket Telefon Numarası")]
        [RegularExpression(@"^(0(\d{3}) (\d{3}) (\d{2}) (\d{2}))$", ErrorMessage = "Telefon numarası uygun formatta girilmemiş...")]
        public string CompanyPhone { get; set; }

        [Required(ErrorMessage = "Şirket Adresi gereklidir.")]
        [Display(Name = "Şirket Adresi")]
        public string CompanyAdress { get; set; }

    }
}
