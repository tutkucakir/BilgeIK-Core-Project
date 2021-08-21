using System.ComponentModel.DataAnnotations;

namespace BilgeIK.CORE.Entity.Enums
{
    public enum DefinitionType
    {
        [Display(Name ="İzinler")]
        Leave=1,
        [Display(Name ="Raporlar")]
        Report=2
    }
}
