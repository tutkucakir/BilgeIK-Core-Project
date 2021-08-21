using System.ComponentModel.DataAnnotations;

namespace BilgeIK.CORE.Entity.Enums
{
    public enum Status
    {
        [Display(Name = "Onay Bekliyor")]
        Pending =0,
        [Display(Name = "Aktif")]
        Actived =1,
        [Display(Name = "Deaktif")]
        DeActived =2,
        [Display(Name = "Silinmiş")]
        Deleted =3
    }
}
