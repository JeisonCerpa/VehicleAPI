using Microsoft.AspNetCore.Mvc;
using VehicleAPI.Data;
using VehicleAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

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

    private void EnviarAlertaEmail(string destinatario, string asunto, string mensaje)
    {
        var smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "smtp.zoho.com";
        var smtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587");
        var smtpUser = Environment.GetEnvironmentVariable("SMTP_USER") ?? "";
        var smtpPass = Environment.GetEnvironmentVariable("SMTP_PASS") ?? "";

        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(smtpUser));
        email.To.Add(MailboxAddress.Parse(destinatario));
        email.Subject = asunto;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = mensaje };

        using var smtp = new SmtpClient();
        // smtp.ServerCertificateValidationCallback = (s, c, h, e) => true; // Validación activada
        smtp.Connect(smtpHost, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
        smtp.Authenticate(smtpUser, smtpPass);
        smtp.Send(email);
        smtp.Disconnect(true);
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

        // Validar duplicados por Placa y MarcaTemporal
        bool existe = await _context.Registros.AnyAsync(r => r.Placa == placaFormateada && r.MarcaTemporal == marcaTemporal);
        if (existe)
            return Conflict("Ya existe un registro con la misma placa y fecha.");

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

        // Alerta detallada si hay algún campo crítico con "Insuficiente" o "No"
        var camposCriticos = new (string Nombre, string? Valor)[] {
            ("Carrocería Latonería y Puertas", registro.CarroceriaLatoneriaPuertas),
            ("Carrocería Marco y Carcasa (Motos)", registro.CarroceriaMarcoCarcasaMotos),
            ("Carrocería Suspensión", registro.CarroceriaSuspension),
            ("Carrocería Asientos y Cojinería", registro.CarroceriaAsientosCojineria),
            ("Carrocería Seguros/Bloqueos Puertas", registro.CarroceriaSegurosBloqueosPuertas),
            ("Carrocería Limpiabrisas", registro.CarroceriaLimpiabrisas),
            ("Carrocería Vidrios", registro.CarroceriaVidrios),
            ("Carrocería Retrovisores", registro.CarroceriaRetrovisores),
            ("Elementos Labrado Llantas", registro.ElementosLabradoLlantas),
            ("Elementos Presión Llantas", registro.ElementosPresionLlantas),
            ("Elementos Nivel Líquidos", registro.ElementosNivelLiquidos),
            ("Elementos Ausencia Fugas", registro.ElementosAusenciaFugas),
            ("Elementos Pedales", registro.ElementosPedales),
            ("Elementos Frenos Disco (Motos)", registro.ElementosFrenosDiscoMotos),
            ("Elementos Cadena (Motos)", registro.ElementosCadenaMotos),
            ("Elementos Manijas Freno/Clutch (Motos)", registro.ElementosManijasFrenoClutchMotos),
            ("Eléctrico Farolas", registro.ElectricoFarolas),
            ("Eléctrico Direccionales/Parqueo", registro.ElectricoDireccionalesParqueo),
            ("Eléctrico Luces Reversa", registro.ElectricoLucesReversa),
            ("Eléctrico Luces Freno/Pare", registro.ElectricoLucesFrenoPare),
            ("Eléctrico Indicadores Tablero", registro.ElectricoIndicadoresTablero),
            ("Eléctrico Pito", registro.ElectricoPito),
            ("Eléctrico Alarma Reversa", registro.ElectricoAlarmaReversa),
            ("Prevención Cinturones Seguridad", registro.PrevencionCinturonesSeguridad),
            ("Prevención Alarma Seguridad", registro.PrevencionAlarmaSeguridad),
            ("Prevención Kit Carretera", registro.PrevencionKitCarretera),
            ("Prevención Extintor", registro.PrevencionExtintor),
            ("Prevención Llanta Repuesto", registro.PrevencionLlantaRepuesto),
            ("Prevención Chaleco Reflectivo", registro.PrevencionChalecoReflectivo),
            ("Prevención Casco", registro.PrevencionCasco),
            ("Tarjeta Propiedad", registro.TarjetaPropiedad),
            ("SOAT", registro.Soat),
            ("Seguridad Social", registro.SeguridadSocial),
            ("Licencia Conducción", registro.LicenciaConduccion),
            ("Certificado Revisión", registro.CertificadoRevision),
            ("Pertenece Empresa", registro.PerteneceEmpresa),
            ("Vehículo Autorizado Transitar", registro.VehiculoAutorizadoTransitar)
        };

        var criticos = camposCriticos.Where(c => c.Valor == "Insuficiente" || c.Valor == "No").ToList();

        var alertaDestinatario = Environment.GetEnvironmentVariable("ALERTA_DESTINATARIO") ?? "destino@correo.com";

        if (criticos.Any())
        {
            string mensaje = $"{DateTime.Now:dd/MM/yyyy HH:mm:ss}\n\n" +
                "Se detectaron problemas en la inspección del vehículo.\n\n" +
                $"Placa: {registro.Placa}\n" +
                $"Conductor: {registro.NombreConductor}\n\n" +
                "Los siguientes ítems presentan problemas:\n";
            foreach (var c in criticos)
                mensaje += $"- {c.Nombre}: {c.Valor}\n";
            if (!string.IsNullOrWhiteSpace(registro.FactoresImpidenMovilizacion))
                mensaje += $"\nFactores que impiden su movilización: {registro.FactoresImpidenMovilizacion}";
            if (!string.IsNullOrWhiteSpace(registro.Observaciones))
                mensaje += $"\nObservaciones: {registro.Observaciones}";
            EnviarAlertaEmail(alertaDestinatario, "Alerta de vehículo con problemas", mensaje);
        }

        return Ok();
    }

    [HttpGet]
    public ActionResult<IEnumerable<RegistroVehiculo>> Get()
    {
        return Ok(_context.Registros.ToList());
    }
}
