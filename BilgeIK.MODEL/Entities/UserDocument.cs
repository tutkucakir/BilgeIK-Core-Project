using BilgeIK.CORE.Entity.Concrete;
using System;
using System.ComponentModel.DataAnnotations;

namespace BilgeIK.MODEL.Entities
{
    public class UserDocument : CoreEntity
    {
        public string Name { get; set; }
        public string FileUrl { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }
        
    }
}
