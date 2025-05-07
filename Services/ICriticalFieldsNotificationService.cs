using VehicleAPI.Models;

namespace VehicleAPI.Services;

public interface ICriticalFieldsNotificationService
{
    /// <summary>
    /// Verifica si un registro tiene campos críticos con valores "Insuficiente" o "No"
    /// </summary>
    /// <param name="registro">El registro de vehículo a verificar</param>
    /// <returns>Una lista de campos críticos con problemas</returns>
    List<(string Nombre, string? Valor)> ValidateCriticalFields(RegistroVehiculo registro);

    /// <summary>
    /// Formatea un mensaje de alerta para los campos críticos
    /// </summary>
    /// <param name="registro">El registro de vehículo</param>
    /// <param name="camposCriticos">Lista de campos críticos con problemas</param>
    /// <returns>Mensaje formateado</returns>
    string FormatAlertMessage(RegistroVehiculo registro, List<(string Nombre, string? Valor)> camposCriticos);

    /// <summary>
    /// Verifica campos críticos y envía una alerta si es necesario
    /// </summary>
    /// <param name="registro">El registro de vehículo</param>
    /// <param name="esSincronizacion">Indica si es una sincronización masiva</param>
    /// <returns>True si se envió una alerta, False en caso contrario</returns>
    bool ProcessCriticalFieldsAndNotify(RegistroVehiculo registro, bool? esSincronizacion);
}

