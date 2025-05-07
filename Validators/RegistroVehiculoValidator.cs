using FluentValidation;
using VehicleAPI.Models;

namespace VehicleAPI.Validators;

public class RegistroVehiculoValidator : AbstractValidator<RegistroVehiculo>
{
    public RegistroVehiculoValidator()
    {
        // Sin reglas: todas las validaciones se delegan al formulario de Google.
    }
}
