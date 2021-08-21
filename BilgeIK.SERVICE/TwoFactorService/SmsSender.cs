using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeIK.SERVICE.TwoFactorService
{
    public class SmsSender
    {

        private readonly TwoFactorOptions _twoFactorOptions;
        private readonly TwoFactorService _twoFactorService;

        public SmsSender(IOptions<TwoFactorOptions> options, TwoFactorService twoFactorService)
        {
            _twoFactorOptions = options.Value;
            _twoFactorService = twoFactorService;
        }

        public string Send(string phone)
        {
            string code = _twoFactorService.GetCodeVerification().ToString();
            //SMS Provider kodları

            //return code;
            return "2222";
        }


    }
}
