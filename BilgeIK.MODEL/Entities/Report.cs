using BilgeIK.CORE.Entity.Concrete;
using System;
using System.ComponentModel.DataAnnotations;

namespace BilgeIK.MODEL.Entities
{
    public class Report : CoreEntity
    {
        public string Hospital { get; set; }
        public string Condition { get; set; }
        public string DoctorName { get; set; }
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }
        public int? PlannedDay { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ExpiresDate { get; set; }
        public int? UsedDay { get; set; }
        public string Notes { get; set; }

        public Guid DefinitionId { get; set; }
        public Definition Definition { get; set; }

        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
