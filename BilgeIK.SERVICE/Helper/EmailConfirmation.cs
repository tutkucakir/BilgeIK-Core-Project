using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BilgeIK.SERVICE.Helper
{
    public static class EmailConfirmation
    {
        public static void SendEmail(string link, string email)
        {
            try
            {
                MailMessage mail = new MailMessage();

                SmtpClient smtpClient = new SmtpClient("mail.domain.com");
                mail.From = new MailAddress("noreply@domain.com", "Gönderici Başlık");
                mail.To.Add(email);
                mail.Subject = "E-Posta Doğrulama İşlemleri";
                mail.Body = "<h3>E-Posta adresinizi doğrulamak için lütfen aşağıdaki linke tıklayınız. </h3><hr />";
                mail.Body += $"<a href='{link}'>E-Posta Doğrulama Linki</a>";
                mail.IsBodyHtml = true;

                smtpClient.Port = 587;
                smtpClient.Credentials = new System.Net.NetworkCredential("noreply@domain.com", "123");
                smtpClient.Send(mail);
            }
            catch (Exception)
            {
                //
            }


        }
    }
}
