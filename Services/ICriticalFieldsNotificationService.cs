using VehicleAPI.Models;

namespace VehicleAPI.Services;

public interface ICriticalFieldsNotificationService
{
    /// Verifica si un registro tiene campos críticos con valores "Insuficiente" o "No"
    /// El registro de vehículo a verificar
    /// Una lista de campos críticos con problemas
    List<(string Nombre, string? Valor)> ValidateCriticalFields(RegistroVehiculo registro);

    /// Formatea un mensaje de alerta para los campos críticos
    /// El registro de vehículo
    /// Lista de campos críticos con problemas
    /// Mensaje formateado
    string FormatAlertMessage(RegistroVehiculo registro, List<(string Nombre, string? Valor)> camposCriticos);

    /// Verifica campos críticos y envía una alerta si es necesario
    /// El registro de vehículo
    /// Indica si es una sincronización masiva
    /// True si se envió una alerta, False en caso contrario
    bool ProcessCriticalFieldsAndNotify(RegistroVehiculo registro, bool? esSincronizacion);
}

