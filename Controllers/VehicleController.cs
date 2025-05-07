using Microsoft.AspNetCore.Mvc;
using VehicleAPI.Data;
using VehicleAPI.Models;
using Microsoft.EntityFrameworkCore;
using VehicleAPI.Services;

namespace VehicleAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly VehicleDbContext _context;
    private readonly IVehicleDataFormatterService _formatterService;
    private readonly ICriticalFieldsNotificationService _criticalFieldsService;
    private readonly ILogger<VehicleController> _logger;

    public VehicleController(
        VehicleDbContext context, 
        IVehicleDataFormatterService formatterService,
        ICriticalFieldsNotificationService criticalFieldsService,
        ILogger<VehicleController> logger)
    {
        _context = context;
        _formatterService = formatterService;
        _criticalFieldsService = criticalFieldsService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] RegistroVehiculoDto registro)
    {
        try
        {
            // Formatear DTO a entidad usando el servicio
            var nuevoRegistro = _formatterService.FormatRegistroVehiculo(registro);

            // Guardar en base de datos solo si la variable de entorno lo permite
            var guardarEnBd = Environment.GetEnvironmentVariable("GUARDAR_EN_BD") ?? "true";
            if (guardarEnBd.ToLower() == "true")
            {
                _context.Registros.Add(nuevoRegistro);
                await _context.SaveChangesAsync();
            }
            
            // Procesar campos críticos y enviar notificaciones
            bool notificationSent = _criticalFieldsService.ProcessCriticalFieldsAndNotify(nuevoRegistro, registro.EsSync);
            
            // Devolver respuesta con información adicional
            return Ok(new 
            {
                Message = "Validación exitosa (no se guardó en base de datos)",
                Placa = nuevoRegistro.Placa,
                FechaRegistro = nuevoRegistro.MarcaTemporal,
                NotificacionEnviada = notificationSent
            });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Error de argumento al procesar registro");
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar registro de vehículo");
            return StatusCode(500, new { Message = "Error interno al procesar el registro" });
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<RegistroVehiculo>> Get()
    {
        try
        {
            var registros = _context.Registros.ToList();
            return Ok(new { 
                Count = registros.Count,
                Data = registros 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al consultar registros");
            return StatusCode(500, new { Message = "Error al obtener los registros" });
        }
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<RegistroVehiculo>> GetById(int id)
    {
        try
        {
            var registro = await _context.Registros.FindAsync(id);
            
            if (registro == null)
                return NotFound(new { Message = $"No se encontró registro con ID: {id}" });
                
            return Ok(registro);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al consultar registro por ID {Id}", id);
            return StatusCode(500, new { Message = "Error al obtener el registro" });
        }
    }
}
