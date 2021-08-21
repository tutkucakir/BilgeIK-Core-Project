using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BilgeIK.SERVICE.Helper
{
    public static class PasswordReset
    {

        public static void PasswordResetSendEmail(string link, string email)
        {
            try
            {
                MailMessage mail = new MailMessage();

                SmtpClient smtpClient = new SmtpClient("mail.domain.com");
                mail.From = new MailAddress("noreply@domain.com", "Gönderici Başlığı");
                mail.To.Add(email);
                mail.Subject = "Parola Sıfırlama Talebiniz Hk.";
                mail.Body = "<h3>Parolanızı yenilemek için lütfen aşağıdaki linke tıklayınız. </h3><hr />";
                mail.Body += $"<a href='{link}'>Parola Yenileme Linki</a>";
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
