using MailKit.Net.Smtp;
using MimeKit;

namespace BureauOrdreDigital.Api.Shared.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var host = _configuration["Smtp:Host"]
            ?? throw new InvalidOperationException("Smtp:Host non configuré.");
        var port = int.Parse(_configuration["Smtp:Port"]
            ?? throw new InvalidOperationException("Smtp:Port non configuré."));
        var username = _configuration["Smtp:Username"]
            ?? throw new InvalidOperationException("Smtp:Username non configuré.");
        var password = _configuration["Smtp:Password"]
            ?? throw new InvalidOperationException("Smtp:Password non configuré.");
        var from = _configuration["Smtp:From"]
            ?? throw new InvalidOperationException("Smtp:From non configuré.");

        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(from));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(username, password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}