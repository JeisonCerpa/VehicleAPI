using VehicleAPI.Models;

namespace VehicleAPI.Services;

public interface IVehicleDataFormatterService
{
    // Métodos para formateo de fechas
    DateTime FormatMarcaTemporal(string? fecha);
    DateTime? FormatOptionalDate(string? fecha);
    
    // Método para formateo de placa
    string FormatPlaca(string? placa);
    
    // Método para convertir DTO a entidad
    RegistroVehiculo FormatRegistroVehiculo(RegistroVehiculoDto dto);
}

