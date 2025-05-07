namespace VehicleAPI.Configuration;

public class VehicleSettings
{
    public AlertaSettings Alerta { get; set; } = new AlertaSettings();
    public EmailSettings Email { get; set; } = new EmailSettings();
}

public class AlertaSettings
{
    public string? Destinatario { get; set; }
}

public class EmailSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public string Contraseña { get; set; } = string.Empty;
}

