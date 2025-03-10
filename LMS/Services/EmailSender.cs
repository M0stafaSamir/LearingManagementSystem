using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace LMS.services_for_authentication   
{
    public class EmailSender : IEmailSender
    {
    private readonly string _smtpServer = "smtp.gmail.com";  // Gmail SMTP
    private readonly int _smtpPort = 587; // Use port 587 with TLS
    private readonly string _emailFrom = "kk2573504@gmail.com"; // Your Gmail
    private readonly string _emailPassword = "ovfo buuw jlfq mlsp"; // Use Gmail App Password

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        using (var client = new SmtpClient(_smtpServer, _smtpPort))
        {
            client.Credentials = new NetworkCredential(_emailFrom, _emailPassword);
            client.EnableSsl = true;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailFrom),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }
    }
}
}
