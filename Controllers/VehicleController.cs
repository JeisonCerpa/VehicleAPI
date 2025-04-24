using Microsoft.AspNetCore.Mvc;
using VehicleAPI.Data;
using VehicleAPI.Models;

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
    public async Task<IActionResult> Post([FromBody] RegistroVehiculo registro)
    {
        _context.Registros.Add(registro);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet]
    public ActionResult<IEnumerable<RegistroVehiculo>> Get()
    {
        return Ok(_context.Registros.ToList());
    }
}
