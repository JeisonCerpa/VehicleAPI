public class FuelSupplyRecord
{
    public int Id { get; set; }
    public DateTime MarcaTemporal { get; set; }
    public string? PlacasDelVehiculo { get; set; }
    public string? TipoCombustible { get; set; }
    public int? Kilometraje { get; set; }
    public decimal? CantidadGalones { get; set; }
    public decimal? ValorCombustible { get; set; }
    public string? DiligenciadoPor { get; set; }
}

public class FuelSupplyRecordDto
{
    public string MarcaTemporal { get; set; } = string.Empty;
    public string? PlacasDelVehiculo { get; set; }
    public string? TipoCombustible { get; set; }
    public int? Kilometraje { get; set; }
    public decimal? CantidadGalones { get; set; }
    public decimal? ValorCombustible { get; set; }
    public string? DiligenciadoPor { get; set; }
    public bool? EsSync { get; set; } // Nuevo campo para distinguir sincronización
}