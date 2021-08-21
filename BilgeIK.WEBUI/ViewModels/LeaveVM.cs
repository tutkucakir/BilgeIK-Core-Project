using BilgeIK.CORE.Entity.Enums;
using BilgeIK.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewModels
{
    public class LeaveVM
    {
        [Display(Name = "İzin Başlama Tarihi")]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }

        [Display(Name = "İzin Bitiş Tarihi(Dahil)")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        [Display(Name ="Planlanan Gün Sayısı")]
        public int? PlannedDay { get; set; }
        [Display(Name = "İzin Dönüşü Başlama Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? ExpiresDate { get; set; }
        [Display(Name = "Kullanılan Gün")]
        public int? UsedDay { get; set; }
        [Display(Name = "Açıklamalar")]
        public string Notes { get; set; }
        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }
        [Display(Name = "İzin Türü")]
        public Guid DefinitionId { get; set; }
        public List<Definition> Definition { get; set; }
        public Status Status { get; set; }
        public string Id { get; set; }
    }
}
