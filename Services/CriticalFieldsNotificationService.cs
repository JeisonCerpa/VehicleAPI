using Microsoft.Extensions.Configuration;
using VehicleAPI.Models;

namespace VehicleAPI.Services;

public class CriticalFieldsNotificationService : ICriticalFieldsNotificationService
{
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly string _alertaDestinatario;

    public CriticalFieldsNotificationService(IEmailService emailService, IConfiguration configuration)
    {
        _emailService = emailService;
        _configuration = configuration;
        
        // Obtener el destinatario de las alertas desde la configuración
        _alertaDestinatario = Environment.GetEnvironmentVariable("ALERTA_DESTINATARIO") 
            ?? _configuration["AlertaSettings:Destinatario"] 
            ?? "destino@correo.com";
    }
    /// Define los campos críticos y sus propiedades correspondientes en el modelo
    /// Array de campos críticos con nombres y propiedades
    private (string Nombre, string? Valor)[] GetCriticalFieldDefinitions(RegistroVehiculo registro)
    {
        return new (string Nombre, string? Valor)[] {
            // Categoría Carrocería
            ("Carrocería Latonería y Puertas", registro.CarroceriaLatoneriaPuertas),
            ("Carrocería Marco y Carcasa (Motos)", registro.CarroceriaMarcoCarcasaMotos),
            ("Carrocería Suspensión", registro.CarroceriaSuspension),
            ("Carrocería Asientos y Cojinería", registro.CarroceriaAsientosCojineria),
            ("Carrocería Seguros/Bloqueos Puertas", registro.CarroceriaSegurosBloqueosPuertas),
            ("Carrocería Limpiabrisas", registro.CarroceriaLimpiabrisas),
            ("Carrocería Vidrios", registro.CarroceriaVidrios),
            ("Carrocería Retrovisores", registro.CarroceriaRetrovisores),
            
            // Categoría Elementos
            ("Elementos Labrado Llantas", registro.ElementosLabradoLlantas),
            ("Elementos Presión Llantas", registro.ElementosPresionLlantas),
            ("Elementos Nivel Líquidos", registro.ElementosNivelLiquidos),
            ("Elementos Ausencia Fugas", registro.ElementosAusenciaFugas),
            ("Elementos Pedales", registro.ElementosPedales),
            ("Elementos Frenos Disco (Motos)", registro.ElementosFrenosDiscoMotos),
            ("Elementos Cadena (Motos)", registro.ElementosCadenaMotos),
            ("Elementos Manijas Freno/Clutch (Motos)", registro.ElementosManijasFrenoClutchMotos),
            
            // Categoría Eléctrico
            ("Eléctrico Farolas", registro.ElectricoFarolas),
            ("Eléctrico Direccionales/Parqueo", registro.ElectricoDireccionalesParqueo),
            ("Eléctrico Luces Reversa", registro.ElectricoLucesReversa),
            ("Eléctrico Luces Freno/Pare", registro.ElectricoLucesFrenoPare),
            ("Eléctrico Indicadores Tablero", registro.ElectricoIndicadoresTablero),
            ("Eléctrico Pito", registro.ElectricoPito),
            ("Eléctrico Alarma Reversa", registro.ElectricoAlarmaReversa),
            
            // Categoría Prevención
            ("Prevención Cinturones Seguridad", registro.PrevencionCinturonesSeguridad),
            ("Prevención Alarma Seguridad", registro.PrevencionAlarmaSeguridad),
            ("Prevención Kit Carretera", registro.PrevencionKitCarretera),
            ("Prevención Extintor", registro.PrevencionExtintor),
            ("Prevención Llanta Repuesto", registro.PrevencionLlantaRepuesto),
            ("Prevención Chaleco Reflectivo", registro.PrevencionChalecoReflectivo),
            ("Prevención Casco", registro.PrevencionCasco),
            
            // Categoría Documentos
            ("Tarjeta Propiedad", registro.TarjetaPropiedad),
            ("SOAT", registro.Soat),
            ("Seguridad Social", registro.SeguridadSocial),
            ("Licencia Conducción", registro.LicenciaConduccion),
            ("Certificado Revisión", registro.CertificadoRevision),
            ("Pertenece Empresa", registro.PerteneceEmpresa),
            ("Vehículo Autorizado Transitar", registro.VehiculoAutorizadoTransitar)
        };
    }
    /// Obtiene los valores críticos que indican un problema
    /// Array de valores considerados críticos
    private string[] GetCriticalValues()
    {
        // En el futuro, esto podría venir de la configuración
        return new[] { "Insuficiente", "No" };
    }

    /// Verifica si un registro tiene campos críticos con valores problemáticos
    public List<(string Nombre, string? Valor)> ValidateCriticalFields(RegistroVehiculo registro)
    {
        var camposCriticos = GetCriticalFieldDefinitions(registro);
        var valoresCriticos = GetCriticalValues();
        
        // Filtrar campos que tienen valores críticos
        var camposConProblemas = camposCriticos
            .Where(c => valoresCriticos.Contains(c.Valor))
            .ToList();
            
        return camposConProblemas;
    }

    /// Formatea un mensaje de alerta para los campos críticos con problemas

    public string FormatAlertMessage(RegistroVehiculo registro, List<(string Nombre, string? Valor)> camposCriticos)
    {
        string mensaje = $"{registro.MarcaTemporal:dd/MM/yyyy HH:mm:ss}\n\n" +
            "Se detectaron problemas en la inspección del vehículo.\n\n" +
            $"Placa: {registro.Placa}\n" +
            $"Conductor: {registro.NombreConductor}\n\n" +
            "Los siguientes ítems presentan problemas:\n";
        
        // Agregar cada campo crítico al mensaje
        foreach (var c in camposCriticos)
            mensaje += $"- {c.Nombre}: {c.Valor}\n";
        
        // Agregar información adicional si existe
        if (!string.IsNullOrWhiteSpace(registro.FactoresImpidenMovilizacion))
            mensaje += $"\nFactores que impiden su movilización: {registro.FactoresImpidenMovilizacion}";
        
        if (!string.IsNullOrWhiteSpace(registro.Observaciones))
            mensaje += $"\nObservaciones: {registro.Observaciones}";
            
        return mensaje;
    }

    /// Procesa los campos críticos y envía una notificación si es necesario
    public bool ProcessCriticalFieldsAndNotify(RegistroVehiculo registro, bool? esSincronizacion)
    {
        // Validar campos críticos
        var camposConProblemas = ValidateCriticalFields(registro);
        
        // Solo enviar correo si hay campos con problemas y NO es sincronización masiva
        if ((esSincronizacion == null || esSincronizacion == false) && camposConProblemas.Any())
        {
            // Formatear mensaje
            string mensaje = FormatAlertMessage(registro, camposConProblemas);
            
            // Enviar alerta
            _emailService.EnviarAlertaEmail(
                _alertaDestinatario, 
                "Alerta de vehículo con problemas", 
                mensaje
            );
            
            return true;
        }
        
        return false;
    }
}

