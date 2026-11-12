using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Fundoo.Notification.Service;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string messageBody)
    {
        var emailSettings = _configuration.GetSection("SmtpSettings");

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(emailSettings["SenderName"], emailSettings["SenderEmail"]));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = messageBody };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
        await smtp.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["Password"]);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}