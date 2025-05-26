using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using VehicleAPI.Models;

namespace VehicleAPI.Data;

public class VehicleDbContext : DbContext
{
    public VehicleDbContext(DbContextOptions<VehicleDbContext> options) : base(options) { }
    public DbSet<RegistroVehiculo> Registros { get; set; }
    public DbSet<FuelSupplyRecord> FuelSupplies { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "name=DefaultConnection";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
