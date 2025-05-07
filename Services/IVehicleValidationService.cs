using VehicleAPI.Models;

namespace VehicleAPI.Services;

public interface IVehicleValidationService
{
    /// <summary>
    /// Verifica si existe un registro duplicado con la misma placa y marca temporal
    /// </summary>
    /// <param name="registro">El registro a verificar</param>
    /// <returns>True si existe un duplicado, False si no existe</returns>
    Task<bool> ExisteDuplicado(RegistroVehiculo registro);
    
    /// <summary>
    /// Valida un registro completo según reglas de negocio
    /// </summary>
    /// <param name="registro">El registro a validar</param>
    /// <returns>Un objeto ValidationResult con los errores encontrados</returns>
    Task<ValidationResult> ValidarRegistro(RegistroVehiculo registro);
    
    /// <summary>
    /// Valida las fechas del registro para asegurar consistencia
    /// </summary>
    /// <param name="registro">El registro a validar</param>
    /// <returns>Lista de errores de validación de fechas</returns>
    List<string> ValidarFechas(RegistroVehiculo registro);
}

/// <summary>
/// Resultado de validación con información detallada de errores
/// </summary>
public class ValidationResult
{
    public bool IsValid => !Errors.Any();
    public List<ValidationError> Errors { get; } = new List<ValidationError>();
    
    public void AddError(string propertyName, string errorMessage)
    {
        Errors.Add(new ValidationError
        {
            PropertyName = propertyName,
            ErrorMessage = errorMessage
        });
    }
}

/// <summary>
/// Error específico de validación
/// </summary>
public class ValidationError
{
    public string PropertyName { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}

