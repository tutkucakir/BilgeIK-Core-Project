using BilgeIK.CORE.Entity.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeIK.MODEL.Entities
{
    public class BankAccount : CoreEntity
    {
        [MaxLength(400)]
        public string IBAN { get; set; }
        [MaxLength(400)]
        public string Bank { get; set; }
        [MaxLength(400)]
        public string BankBranch { get; set; }
        public int AccountNumber { get; set; }

        [MaxLength(500)]
        public string AccountHolderInfo { get; set; }
    }
}
