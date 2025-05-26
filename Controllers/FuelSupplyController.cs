using Microsoft.AspNetCore.Mvc;
using VehicleAPI.Data;
using VehicleAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace VehicleAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FuelSupplyController : ControllerBase
{
    private readonly VehicleDbContext _context;
    private readonly ILogger<FuelSupplyController> _logger;

    public FuelSupplyController(VehicleDbContext context, ILogger<FuelSupplyController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] FuelSupplyRecordDto dto)
    {
        try
        {
            var record = new FuelSupplyRecord
            {
                MarcaTemporal = DateTime.Parse(dto.MarcaTemporal),
                PlacasDelVehiculo = dto.PlacasDelVehiculo,
                TipoCombustible = dto.TipoCombustible,
                Kilometraje = dto.Kilometraje,
                CantidadGalones = dto.CantidadGalones,
                ValorCombustible = dto.ValorCombustible,
                DiligenciadoPor = dto.DiligenciadoPor
            };
            _logger.LogInformation("Intentando guardar registro: {@Record}", record);
            _context.FuelSupplies.Add(record);
            await _context.SaveChangesAsync();
            return Ok(new { Message = dto.EsSync == true ? "Validación exitosa (no se guardó en base de datos)" : "Registro de abastecimiento guardado", Id = record.Id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar registro de abastecimiento: {Error}", ex.Message);
            // Para depuración, devolvemos el mensaje de error interno (no dejar en producción)
            return StatusCode(500, new { Message = "Error interno al guardar el registro", Error = ex.ToString() });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var registros = await _context.FuelSupplies.ToListAsync();
        return Ok(new { Count = registros.Count, Data = registros });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var registro = await _context.FuelSupplies.FindAsync(id);
        if (registro == null)
            return NotFound(new { Message = $"No se encontró registro con ID: {id}" });
        return Ok(registro);
    }
}
