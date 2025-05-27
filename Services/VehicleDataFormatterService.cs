using System.Globalization;
using System.Text.RegularExpressions;
using VehicleAPI.Models;

namespace VehicleAPI.Services;

public class VehicleDataFormatterService : IVehicleDataFormatterService
{
    private static readonly string[] FormatosFecha = new[] {
        "d/M/yyyy H:mm:ss", "dd/M/yyyy H:mm:ss", 
        "d/MM/yyyy H:mm:ss", "dd/MM/yyyy H:mm:ss",
        "d/M/yyyy HH:mm:ss", "dd/M/yyyy HH:mm:ss",
        "d/M/yyyy", "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy",
        "yyyy-MM-dd", "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-ddTHH:mm:ssZ"
    };

    private readonly TimeZoneInfo _colombiaTimeZone;

    public VehicleDataFormatterService()
    {
        _colombiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
    }

    public DateTime FormatMarcaTemporal(string? fecha)
    {
        if (string.IsNullOrWhiteSpace(fecha))
            throw new ArgumentException("La marca temporal no puede estar vacía");

        if (!DateTime.TryParseExact(fecha, FormatosFecha, 
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultado) 
            && !DateTime.TryParse(fecha, out resultado))
        {
            throw new ArgumentException("Formato de fecha inválido para MarcaTemporal");
        }

        // Convertir a zona horaria de Colombia y devolver como UTC
        var colombiaTime = TimeZoneInfo.ConvertTime(resultado, _colombiaTimeZone);
        return DateTime.SpecifyKind(colombiaTime, DateTimeKind.Utc);
    }

    public DateTime? FormatOptionalDate(string? fecha)
    {
        if (string.IsNullOrWhiteSpace(fecha))
            return null;

        if (DateTime.TryParseExact(fecha, FormatosFecha, 
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultado) 
            || DateTime.TryParse(fecha, out resultado))
        {
            // Convertir a UTC si es posible
            return DateTime.SpecifyKind(resultado, DateTimeKind.Utc);
        }

        throw new ArgumentException($"Formato de fecha inválido: {fecha}");
    }

    public string FormatPlaca(string? placa)
    {
        if (string.IsNullOrWhiteSpace(placa))
            throw new ArgumentException("La placa no puede estar vacía");

        string placaFormateada = Regex.Replace(placa.ToUpper(), "[^A-Z0-9]", "");
        
        if (placaFormateada.Length != 6 || !Regex.IsMatch(placaFormateada, "^[A-Z0-9]{6}$"))
            throw new ArgumentException("La placa debe tener exactamente 6 caracteres alfanuméricos");

        return placaFormateada;
    }

    public RegistroVehiculo FormatRegistroVehiculo(RegistroVehiculoDto dto)
    {
        var registro = new RegistroVehiculo
        {
            MarcaTemporal = FormatMarcaTemporal(dto.MarcaTemporal),
            Placa = FormatPlaca(dto.Placa),
            FechaVencimientoSoat = FormatOptionalDate(dto.FechaVencimientoSoat),
            FechaVencimientoRevision = FormatOptionalDate(dto.FechaVencimientoRevision),
            FechaVencimientoLicencia = FormatOptionalDate(dto.FechaVencimientoLicencia),
            
            // Copiar el resto de propiedades directamente
            NombreConductor = dto.NombreConductor?.Trim(),
            TarjetaPropiedad = dto.TarjetaPropiedad?.Trim(),
            Soat = dto.Soat?.Trim(),
            SeguridadSocial = dto.SeguridadSocial?.Trim(),
            LicenciaConduccion = dto.LicenciaConduccion?.Trim(),
            CertificadoRevision = dto.CertificadoRevision?.Trim(),
            PerteneceEmpresa = dto.PerteneceEmpresa?.Trim(),
            EntregaRecibe = dto.EntregaRecibe?.Trim(),
            FotoFrontalVehiculo = dto.FotoFrontalVehiculo?.Trim(),
            FotoLateralDerechoVehiculo = dto.FotoLateralDerechoVehiculo?.Trim(),
            FotoLateralIzquierdoVehiculo = dto.FotoLateralIzquierdoVehiculo?.Trim(),
            FotoTraseraVehiculo = dto.FotoTraseraVehiculo?.Trim(),
            CarroceriaLatoneriaPuertas = dto.CarroceriaLatoneriaPuertas?.Trim(),
            CarroceriaMarcoCarcasaMotos = dto.CarroceriaMarcoCarcasaMotos?.Trim(),
            CarroceriaSuspension = dto.CarroceriaSuspension?.Trim(),
            CarroceriaAsientosCojineria = dto.CarroceriaAsientosCojineria?.Trim(),
            CarroceriaSegurosBloqueosPuertas = dto.CarroceriaSegurosBloqueosPuertas?.Trim(),
            CarroceriaLimpiabrisas = dto.CarroceriaLimpiabrisas?.Trim(),
            CarroceriaVidrios = dto.CarroceriaVidrios?.Trim(),
            CarroceriaRetrovisores = dto.CarroceriaRetrovisores?.Trim(),
            ElementosLabradoLlantas = dto.ElementosLabradoLlantas?.Trim(),
            ElementosPresionLlantas = dto.ElementosPresionLlantas?.Trim(),
            ElementosNivelLiquidos = dto.ElementosNivelLiquidos?.Trim(),
            ElementosAusenciaFugas = dto.ElementosAusenciaFugas?.Trim(),
            ElementosPedales = dto.ElementosPedales?.Trim(),
            ElementosFrenosDiscoMotos = dto.ElementosFrenosDiscoMotos?.Trim(),
            ElementosCadenaMotos = dto.ElementosCadenaMotos?.Trim(),
            ElementosManijasFrenoClutchMotos = dto.ElementosManijasFrenoClutchMotos?.Trim(),
            ElectricoFarolas = dto.ElectricoFarolas?.Trim(),
            ElectricoDireccionalesParqueo = dto.ElectricoDireccionalesParqueo?.Trim(),
            ElectricoLucesReversa = dto.ElectricoLucesReversa?.Trim(),
            ElectricoLucesFrenoPare = dto.ElectricoLucesFrenoPare?.Trim(),
            ElectricoIndicadoresTablero = dto.ElectricoIndicadoresTablero?.Trim(),
            ElectricoPito = dto.ElectricoPito?.Trim(),
            ElectricoAlarmaReversa = dto.ElectricoAlarmaReversa?.Trim(),
            PrevencionCinturonesSeguridad = dto.PrevencionCinturonesSeguridad?.Trim(),
            PrevencionAlarmaSeguridad = dto.PrevencionAlarmaSeguridad?.Trim(),
            PrevencionKitCarretera = dto.PrevencionKitCarretera?.Trim(),
            PrevencionExtintor = dto.PrevencionExtintor?.Trim(),
            PrevencionLlantaRepuesto = dto.PrevencionLlantaRepuesto?.Trim(),
            PrevencionChalecoReflectivo = dto.PrevencionChalecoReflectivo?.Trim(),
            PrevencionCasco = dto.PrevencionCasco?.Trim(),
            VehiculoAutorizadoTransitar = dto.VehiculoAutorizadoTransitar?.Trim(),
            FactoresImpidenMovilizacion = dto.FactoresImpidenMovilizacion?.Trim(),
            Observaciones = dto.Observaciones?.Trim(),
            NombreConductorCC = dto.NombreConductorCC?.Trim(),
            NombreResponsableVehiculoCC = dto.NombreResponsableVehiculoCC?.Trim()
        };

        return registro;
    }
}

