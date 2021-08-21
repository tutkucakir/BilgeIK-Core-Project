using BilgeIK.CORE.Entity.Abstract;
using BilgeIK.CORE.Entity.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BilgeIK.CORE.Entity.Concrete
{
    public class CoreEntity : IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        public Status Status { get; set; }
    }
}
