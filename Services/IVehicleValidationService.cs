using VehicleAPI.Models;

namespace VehicleAPI.Services;

public interface IVehicleValidationService
{
    /// Verifica si existe un registro duplicado con la misma placa y marca temporal
    /// El registro a verificar
    /// True si existe un duplicado, False si no existe
    Task<bool> ExisteDuplicado(RegistroVehiculo registro);
    

    /// Valida un registro completo según reglas de negocio
    /// El registro a validar
    /// Un objeto ValidationResult con los errores encontrados
    Task<ValidationResult> ValidarRegistro(RegistroVehiculo registro);
    
    /// Valida las fechas del registro para asegurar consistencia
    /// El registro a validar
    /// Lista de errores de validación de fechas
    List<string> ValidarFechas(RegistroVehiculo registro);
}

/// Resultado de validación con información detallada de errores
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

/// Error específico de validación
public class ValidationError
{
    public string PropertyName { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}

