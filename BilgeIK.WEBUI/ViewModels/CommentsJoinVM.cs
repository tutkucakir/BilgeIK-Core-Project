using BilgeIK.CORE.Entity.Enums;
using BilgeIK.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewModels
{
    public class CommentsJoinVM
    {
        public Guid Id { get; set; }
        public string CommentContent { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool FeaturedComment { get; set; }
        public Company Company { get; set; }
        public AppUser AppUser { get; set; }
        public Status Status { get; set; }
    }
}
