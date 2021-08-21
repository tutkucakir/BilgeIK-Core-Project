using BilgeIK.CORE.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BilgeIK.MODEL.Entities
{
    public class SalaryBonus : CoreEntity
    {
        [Column(TypeName = "decimal(18,2)")]
        public Decimal Bonus { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
        public Guid ApproverUserId { get; set; }
        public AppUser AppUser { get; set; }
        public virtual List<SBDocument> SBDocuments { get; set; }

    }
}
