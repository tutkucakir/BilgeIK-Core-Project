using BilgeIK.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BilgeIK.WEBUI.ViewModels
{
    public class ReportJoinUserVM
    {
        public Report Report { get; set; }
        public AppUser AppUser { get; set; }
        public Definition Definition { get; set; }
    }
}
