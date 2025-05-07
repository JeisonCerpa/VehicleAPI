using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using VehicleAPI.Configuration;

namespace VehicleAPI.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<VehicleSettings> settings, ILogger<EmailService> logger)
    {
        var envUser = Environment.GetEnvironmentVariable("SMTP_USER");
        var envPass = Environment.GetEnvironmentVariable("SMTP_PASS");
        var envHost = Environment.GetEnvironmentVariable("SMTP_HOST");
        var envPort = Environment.GetEnvironmentVariable("SMTP_PORT");

        _emailSettings = settings.Value.Email ?? new EmailSettings();
        if (!string.IsNullOrEmpty(envUser))
            _emailSettings.Usuario = envUser;
        if (!string.IsNullOrEmpty(envPass))
            _emailSettings.Contraseña = envPass;
        if (!string.IsNullOrEmpty(envHost))
            _emailSettings.Host = envHost;
        if (!string.IsNullOrEmpty(envPort) && int.TryParse(envPort, out var port))
            _emailSettings.Port = port;
        _logger = logger;
    }

    public void EnviarAlertaEmail(string destinatario, string asunto, string mensaje)
    {
        try
        {
            var email = new MimeMessage();
            
            // Configurar remitente y destinatario
            email.From.Add(MailboxAddress.Parse(_emailSettings.Usuario));
            email.To.Add(MailboxAddress.Parse(destinatario));
            email.Subject = asunto;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = mensaje };

            // Enviar el correo
            using var smtp = new SmtpClient();
            _logger.LogDebug("Conectando al servidor SMTP {Host}:{Port}", _emailSettings.Host, _emailSettings.Port);
            
            smtp.Connect(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            
            if (!string.IsNullOrEmpty(_emailSettings.Usuario) && !string.IsNullOrEmpty(_emailSettings.Contraseña))
            {
                smtp.Authenticate(_emailSettings.Usuario, _emailSettings.Contraseña);
            }
            
            smtp.Send(email);
            smtp.Disconnect(true);
            
            _logger.LogInformation("Email enviado exitosamente a {Destinatario} con asunto {Asunto}", 
                destinatario, asunto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar correo a {Destinatario}: {Error}", 
                destinatario, ex.Message);
            
            // No lanzamos la excepción para no interrumpir el flujo de la aplicación
            // pero dejamos registro del error
        }
    }
}
