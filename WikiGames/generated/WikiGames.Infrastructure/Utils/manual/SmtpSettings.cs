using Microsoft.Extensions.Configuration;

namespace WikiGames.Infrastructure.Utils.Manual;

/// <summary>
/// Configuración SMTP. Se lee de la sección "Smtp" de appsettings.json;
/// si falta algún valor se usan estos valores de ejemplo.
/// </summary>
public sealed class SmtpSettings
{
    public string Host { get; set; } = "smtp.example.com";
    public int Port { get; set; } = 587;
    public bool EnableSsl { get; set; } = true;
    public string User { get; set; } = "wikigames@example.com";
    public string Password { get; set; } = "cambiar-esta-contrasena";
    public string FromAddress { get; set; } = "wikigames@example.com";
    public string FromName { get; set; } = "WikiGames";

    public static SmtpSettings FromConfiguration(IConfiguration? configuration)
    {
        var settings = new SmtpSettings();
        if (configuration is null)
        {
            return settings;
        }

        IConfigurationSection section = configuration.GetSection("Smtp");
        settings.Host = section["Host"] ?? settings.Host;
        settings.Port = int.TryParse(section["Port"], out int port) ? port : settings.Port;
        settings.EnableSsl = bool.TryParse(section["EnableSsl"], out bool ssl) ? ssl : settings.EnableSsl;
        settings.User = section["User"] ?? settings.User;
        settings.Password = section["Password"] ?? settings.Password;
        settings.FromAddress = section["FromAddress"] ?? settings.FromAddress;
        settings.FromName = section["FromName"] ?? settings.FromName;
        return settings;
    }
}
