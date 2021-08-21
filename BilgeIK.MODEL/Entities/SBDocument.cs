using BilgeIK.CORE.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeIK.MODEL.Entities
{
    public class SBDocument : CoreEntity
    {
        [MaxLength(400)]
        public string File { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        public Guid SalaryBonusId { get; set; }
        public SalaryBonus SalaryBonus { get; set; }
    }
}
