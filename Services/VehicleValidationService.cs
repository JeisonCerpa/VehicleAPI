using FluentValidation;
using Microsoft.EntityFrameworkCore;
using VehicleAPI.Data;
using VehicleAPI.Models;

namespace VehicleAPI.Services;

public class VehicleValidationService : IVehicleValidationService
{
    private readonly VehicleDbContext _dbContext;
    private readonly IValidator<RegistroVehiculo> _validator;
    private readonly ILogger<VehicleValidationService> _logger;

    public VehicleValidationService(
        VehicleDbContext dbContext, 
        IValidator<RegistroVehiculo> validator,
        ILogger<VehicleValidationService> logger)
    {
        _dbContext = dbContext;
        _validator = validator;
        _logger = logger;
    }

    /// Verifica si existe un registro duplicado con la misma placa y marca temporal
    public async Task<bool> ExisteDuplicado(RegistroVehiculo registro)
    {
        try
        {
            return await _dbContext.Registros.AnyAsync(r => 
                r.Placa == registro.Placa && 
                r.MarcaTemporal == registro.MarcaTemporal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar duplicados para placa {Placa}", registro.Placa);
            // En caso de error, asumimos que no existe duplicado para no bloquear el proceso
            return false;
        }
    }

    /// Valida un registro completo según reglas de negocio usando FluentValidation
    public async Task<ValidationResult> ValidarRegistro(RegistroVehiculo registro)
    {
        var result = new ValidationResult();
        
        // 1. Validar con FluentValidation
        var fluentResult = await _validator.ValidateAsync(registro);
        
        // Convertir errores de FluentValidation a nuestro formato
        if (!fluentResult.IsValid)
        {
            foreach (var error in fluentResult.Errors)
            {
                result.AddError(error.PropertyName, error.ErrorMessage);
            }
        }
        
        // 2. Validar duplicados
        if (await ExisteDuplicado(registro))
        {
            result.AddError("Placa", $"Ya existe un registro con la placa {registro.Placa} y fecha {registro.MarcaTemporal:dd/MM/yyyy HH:mm:ss}");
        }
        
        // 3. Validar fechas
        foreach (var error in ValidarFechas(registro))
        {
            result.AddError("Fechas", error);
        }
        
        return result;
    }

    /// Realiza validaciones específicas para fechas
    public List<string> ValidarFechas(RegistroVehiculo registro)
    {
        var errores = new List<string>();
        var hoy = DateTime.Today;
        
        // Validar que las fechas de vencimiento sean futuras
        if (registro.FechaVencimientoSoat.HasValue && registro.FechaVencimientoSoat.Value < hoy)
        {
            errores.Add($"La fecha de vencimiento del SOAT ({registro.FechaVencimientoSoat:dd/MM/yyyy}) está vencida");
        }
        
        if (registro.FechaVencimientoRevision.HasValue && registro.FechaVencimientoRevision.Value < hoy)
        {
            errores.Add($"La fecha de vencimiento de la revisión técnico-mecánica ({registro.FechaVencimientoRevision:dd/MM/yyyy}) está vencida");
        }
        
        if (registro.FechaVencimientoLicencia.HasValue && registro.FechaVencimientoLicencia.Value < hoy)
        {
            errores.Add($"La fecha de vencimiento de la licencia de conducción ({registro.FechaVencimientoLicencia:dd/MM/yyyy}) está vencida");
        }
        
        // Validar que la marca temporal no sea futura
        if (registro.MarcaTemporal > DateTime.Now.AddHours(1)) // Permitir cierta diferencia por zonas horarias
        {
            errores.Add($"La fecha y hora de registro ({registro.MarcaTemporal:dd/MM/yyyy HH:mm:ss}) no puede ser futura");
        }
        
        return errores;
    }
}

