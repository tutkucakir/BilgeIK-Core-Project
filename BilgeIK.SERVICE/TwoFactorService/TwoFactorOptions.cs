using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeIK.SERVICE.TwoFactorService
{
    public class TwoFactorOptions
    {
        public string SendGrid_ApiKey { get; set; }
        public int CodeTimeExpire { get; set; }
    }
}
