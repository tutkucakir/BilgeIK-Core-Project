using BilgeIK.CORE.Entity.Enums;
using BilgeIK.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewModels
{
    public class ReportVM
    {
        [Display(Name = "Hastane Adı")]
        [MaxLength(500)]
        [Required(ErrorMessage ="Hastane adını girmelisiniz")]
        public string Hospital { get; set; }
        [Display(Name = "Şikayet/Tanı")]
        [Required(ErrorMessage ="Şikayet alanını doldurmalısınız")]
        public string Condition { get; set; }

        [Display(Name = "Doktor Adı")]
        public string DoctorName { get; set; }
        [Display(Name = "Rap. Başlama Tarihi")]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }
        [Display(Name = "Rap. Bitiş Tarihi")]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }
        [Display(Name = "Raporlu Gün Sayısı")]
        public int? PlannedDay { get; set; }
        [Display(Name = "Rapor Dönüşü Başlama T.")]
        [DataType(DataType.Date)]
        public DateTime? ExpiresDate { get; set; }
        [Display(Name = "Kullanılan Gün")]
        public int? UsedDay { get; set; }
        [Display(Name = "Açıklamalar")]
        public string Notes { get; set; }

        public Guid? DefinitionId { get; set; }
        public List<Definition> Definition { get; set; }

        public Guid? UserId { get; set; }
        public AppUser AppUser { get; set; }
        public Status Status { get; set; }
        public string Id { get; set; }


    }
}
