using Microsoft.EntityFrameworkCore;
using VehicleAPI.Models;

namespace VehicleAPI.Data;

public class VehicleDbContext : DbContext
{
    public DbSet<RegistroVehiculo> Registros { get; set; }

    public VehicleDbContext(DbContextOptions<VehicleDbContext> options) : base(options) { }
}
