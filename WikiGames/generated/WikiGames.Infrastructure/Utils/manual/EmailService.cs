using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using WikiGames.Application.Abstractions;

namespace WikiGames.Infrastructure.Utils.Manual;

/// <summary>
/// Implementación SMTP de IEmailService (Tema 6). La dependencia de System.Net.Mail
/// queda encapsulada exclusivamente en esta clase de la capa Infrastructure.
/// </summary>
public sealed class EmailService : IEmailService
{
    private readonly SmtpSettings _settings;
    private readonly ILogger<EmailService>? _logger;

    public EmailService(SmtpSettings settings, ILogger<EmailService>? logger = null)
    {
        _settings = settings;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(string recipientEmail, string subject, string body)
    {
        if (string.IsNullOrWhiteSpace(recipientEmail))
        {
            return false;
        }

        try
        {
            using var message = new MailMessage
            {
                From = new MailAddress(_settings.FromAddress, _settings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = false,
            };
            message.To.Add(recipientEmail);

            using var client = new SmtpClient(_settings.Host, _settings.Port)
            {
                EnableSsl = _settings.EnableSsl,
                Credentials = new NetworkCredential(_settings.User, _settings.Password),
            };

            await client.SendMailAsync(message);
            return true;
        }
        catch (Exception ex)
        {
            // Un fallo del servidor de correo no debe invalidar la operación de negocio.
            _logger?.LogWarning(ex, "No se pudo enviar el correo a {Recipient}", recipientEmail);
            return false;
        }
    }
}
