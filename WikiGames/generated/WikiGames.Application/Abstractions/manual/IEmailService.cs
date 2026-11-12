namespace WikiGames.Application.Abstractions;

/// <summary>
/// Contrato del servicio externo de correo electrónico (Tema 6).
/// La capa Application solo depende de esta interfaz; la implementación SMTP
/// vive en Infrastructure y se resuelve con IUnitOfWorkUtils.GetRequiredService&lt;IEmailService&gt;().
/// </summary>
public interface IEmailService
{
    Task<bool> SendEmailAsync(string recipientEmail, string subject, string body);
}
