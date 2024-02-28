using MailKit.Net.Smtp;
using MimeKit;

namespace GlowingTemplate.Services
{
    public class EmailService
    {
        public async void SendEmail(string ToMail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("BB103"));
            email.Subject = subject;
            email.To.Add(MailboxAddress.Parse(ToMail));
            email.Body = new TextPart("html") { Text = body };


            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync("glowingtemplates@gmail.com", "fbah ngyd tkwj gtjo");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

        }
    }
}
