using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;

namespace KatFilm
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "Admin@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));  // адрес Email
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.CheckCertificateRevocation = false;
               // отключить  проверками отзыва сертификатов SSL / TLS с не отвечающим центром сертификации.
                await client.ConnectAsync("smtp.gmail.com", 587, false); // 465  передача через smtp сервер google
                await client.AuthenticateAsync("identity183@gmail.com", "oOidentity1");
              //  await client.AuthenticateAsync("identityam@mail.ru", "iI333444!");
              //  await client.AuthenticateAsync("NovitskyAM@gmail.com","oO333444");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
