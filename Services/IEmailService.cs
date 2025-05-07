namespace VehicleAPI.Services;

public interface IEmailService
{
    void EnviarAlertaEmail(string destinatario, string asunto, string mensaje);
}

