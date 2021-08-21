using System.ComponentModel.DataAnnotations;

namespace BilgeIK.CORE.Entity.Enums
{
    public enum Gender
    {
        [Display(Name = "Gizli")]
        Hide =0,
        [Display(Name = "Erkek")]
        Male =1,
        [Display(Name = "Kadın")]
        Female =2
    }
}
