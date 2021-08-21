using BilgeIK.CORE.Entity.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewModels
{
    public class DefinitionVM
    {
        [Display(Name="Tanımlama Adı")]
        [Required(ErrorMessage ="Tanımlama Adı girilmesi zorunludur.")]
        public string Name { get; set; }

        [Display(Name="Tanımlama Alanı")]
        public DefinitionType DefinitionTypes { get; set; }
        public string Id { get; set; }
    }
}
