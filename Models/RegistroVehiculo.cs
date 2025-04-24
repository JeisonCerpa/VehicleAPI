namespace VehicleAPI.Models;

public class RegistroVehiculo
{
    public int Id { get; set; }
    public DateTime MarcaTemporal { get; set; }
    public required string Placa { get; set; }
    public required string NombreConductor { get; set; }
}
