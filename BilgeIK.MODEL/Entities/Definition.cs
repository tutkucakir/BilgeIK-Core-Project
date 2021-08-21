using BilgeIK.CORE.Entity.Concrete;
using BilgeIK.CORE.Entity.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BilgeIK.MODEL.Entities
{
    public class Definition : CoreEntity
    {
        [MaxLength(400)]
        public string Name { get; set; }
        public DefinitionType DefinitionType { get; set; }
        public virtual List<Leave> Leaves { get; set; }
        public virtual List<Report> Reports { get; set; }
    }
}
