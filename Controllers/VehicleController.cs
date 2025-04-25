using Microsoft.AspNetCore.Mvc;
using VehicleAPI.Data;
using VehicleAPI.Models;
using System.Globalization;

namespace VehicleAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly VehicleDbContext _context;

    public VehicleController(VehicleDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RegistroVehiculoDto registro)
    {
        // Formatos posibles de fecha desde Google Sheets y otros sistemas
        var formatosFecha = new[] {
            "d/M/yyyy H:mm:ss", "dd/M/yyyy H:mm:ss", "d/MM/yyyy H:mm:ss", "dd/MM/yyyy H:mm:ss",
            "d/M/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss",
            "d/M/yyyy", "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy",
            "yyyy-MM-dd", "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-ddTHH:mm:ssZ"
        };

        DateTime marcaTemporal;
        if (!DateTime.TryParseExact(registro.MarcaTemporal, formatosFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out marcaTemporal)
            && !DateTime.TryParse(registro.MarcaTemporal, out marcaTemporal))
            return BadRequest("Formato de fecha inválido para MarcaTemporal");

        DateTime? fechaVencimientoSoat = null;
        if (!string.IsNullOrWhiteSpace(registro.FechaVencimientoSoat))
        {
            if (DateTime.TryParseExact(registro.FechaVencimientoSoat, formatosFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out var tempSoat)
                || DateTime.TryParse(registro.FechaVencimientoSoat, out tempSoat))
                fechaVencimientoSoat = tempSoat;
            else
                return BadRequest("Formato de fecha inválido para FechaVencimientoSoat");
        }

        DateTime? fechaVencimientoRevision = null;
        if (!string.IsNullOrWhiteSpace(registro.FechaVencimientoRevision))
        {
            if (DateTime.TryParseExact(registro.FechaVencimientoRevision, formatosFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out var tempRevision)
                || DateTime.TryParse(registro.FechaVencimientoRevision, out tempRevision))
                fechaVencimientoRevision = tempRevision;
            else
                return BadRequest("Formato de fecha inválido para FechaVencimientoRevision");
        }

        DateTime? fechaVencimientoLicencia = null;
        if (!string.IsNullOrWhiteSpace(registro.FechaVencimientoLicencia))
        {
            if (DateTime.TryParseExact(registro.FechaVencimientoLicencia, formatosFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out var tempLicencia)
                || DateTime.TryParse(registro.FechaVencimientoLicencia, out tempLicencia))
                fechaVencimientoLicencia = tempLicencia;
            else
                return BadRequest("Formato de fecha inválido para FechaVencimientoLicencia");
        }

        // Formateo y validación de placa
        if (string.IsNullOrWhiteSpace(registro.Placa))
            return BadRequest("La placa no puede estar vacía o ser nula");

        string placaFormateada = System.Text.RegularExpressions.Regex.Replace(registro.Placa.ToUpper(), "[^A-Z0-9]", "");
        if (placaFormateada.Length != 6 || !System.Text.RegularExpressions.Regex.IsMatch(placaFormateada, "^[A-Z0-9]{6}$"))
            return BadRequest("La placa debe tener exactamente 6 caracteres alfanuméricos, por ejemplo: ABC123");

        var nuevoRegistro = new RegistroVehiculo
        {
            MarcaTemporal = marcaTemporal,
            Placa = placaFormateada,
            NombreConductor = registro.NombreConductor,
            TarjetaPropiedad = registro.TarjetaPropiedad,
            Soat = registro.Soat,
            SeguridadSocial = registro.SeguridadSocial,
            LicenciaConduccion = registro.LicenciaConduccion,
            CertificadoRevision = registro.CertificadoRevision,
            PerteneceEmpresa = registro.PerteneceEmpresa,
            FechaVencimientoSoat = fechaVencimientoSoat,
            FechaVencimientoRevision = fechaVencimientoRevision,
            FechaVencimientoLicencia = fechaVencimientoLicencia,
            EntregaRecibe = registro.EntregaRecibe,
            FotoFrontalVehiculo = registro.FotoFrontalVehiculo,
            FotoLateralDerechoVehiculo = registro.FotoLateralDerechoVehiculo,
            FotoLateralIzquierdoVehiculo = registro.FotoLateralIzquierdoVehiculo,
            FotoTraseraVehiculo = registro.FotoTraseraVehiculo,
            CarroceriaLatoneriaPuertas = registro.CarroceriaLatoneriaPuertas,
            CarroceriaMarcoCarcasaMotos = registro.CarroceriaMarcoCarcasaMotos,
            CarroceriaSuspension = registro.CarroceriaSuspension,
            CarroceriaAsientosCojineria = registro.CarroceriaAsientosCojineria,
            CarroceriaSegurosBloqueosPuertas = registro.CarroceriaSegurosBloqueosPuertas,
            CarroceriaLimpiabrisas = registro.CarroceriaLimpiabrisas,
            CarroceriaVidrios = registro.CarroceriaVidrios,
            CarroceriaRetrovisores = registro.CarroceriaRetrovisores,
            ElementosLabradoLlantas = registro.ElementosLabradoLlantas,
            ElementosPresionLlantas = registro.ElementosPresionLlantas,
            ElementosNivelLiquidos = registro.ElementosNivelLiquidos,
            ElementosAusenciaFugas = registro.ElementosAusenciaFugas,
            ElementosPedales = registro.ElementosPedales,
            ElementosFrenosDiscoMotos = registro.ElementosFrenosDiscoMotos,
            ElementosCadenaMotos = registro.ElementosCadenaMotos,
            ElementosManijasFrenoClutchMotos = registro.ElementosManijasFrenoClutchMotos,
            ElectricoFarolas = registro.ElectricoFarolas,
            ElectricoDireccionalesParqueo = registro.ElectricoDireccionalesParqueo,
            ElectricoLucesReversa = registro.ElectricoLucesReversa,
            ElectricoLucesFrenoPare = registro.ElectricoLucesFrenoPare,
            ElectricoIndicadoresTablero = registro.ElectricoIndicadoresTablero,
            ElectricoPito = registro.ElectricoPito,
            ElectricoAlarmaReversa = registro.ElectricoAlarmaReversa,
            PrevencionCinturonesSeguridad = registro.PrevencionCinturonesSeguridad,
            PrevencionAlarmaSeguridad = registro.PrevencionAlarmaSeguridad,
            PrevencionKitCarretera = registro.PrevencionKitCarretera,
            PrevencionExtintor = registro.PrevencionExtintor,
            PrevencionLlantaRepuesto = registro.PrevencionLlantaRepuesto,
            PrevencionChalecoReflectivo = registro.PrevencionChalecoReflectivo,
            PrevencionCasco = registro.PrevencionCasco,
            VehiculoAutorizadoTransitar = registro.VehiculoAutorizadoTransitar,
            FactoresImpidenMovilizacion = registro.FactoresImpidenMovilizacion,
            Observaciones = registro.Observaciones,
            NombreConductorCC = registro.NombreConductorCC,
            NombreResponsableVehiculoCC = registro.NombreResponsableVehiculoCC
        };
        _context.Registros.Add(nuevoRegistro);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    public ActionResult<IEnumerable<RegistroVehiculo>> Get()
    {
        return Ok(_context.Registros.ToList());
    }
}
