using System.ComponentModel.DataAnnotations.Schema;

public class FuelSupplyRecord
{
    public int Id { get; set; }
    public DateTime MarcaTemporal { get; set; }
    public string? PlacasDelVehiculo { get; set; }
    public string? TipoCombustible { get; set; }
    public int? Kilometraje { get; set; }
    [Column(TypeName = "numeric(10,3)")]
    public decimal? CantidadGalones { get; set; } // decimal(10,3) recomendado
    [Column(TypeName = "numeric(12,2)")]
    public decimal? ValorCombustible { get; set; } // decimal(12,2) recomendado
    public string? DiligenciadoPor { get; set; }
}

public class FuelSupplyRecordDto
{
    public string MarcaTemporal { get; set; } = string.Empty;
    public string? PlacasDelVehiculo { get; set; }
    public string? TipoCombustible { get; set; }
    public int? Kilometraje { get; set; }
    public decimal? CantidadGalones { get; set; } // decimal(10,3) recomendado
    public decimal? ValorCombustible { get; set; } // decimal(12,2) recomendado
    public string? DiligenciadoPor { get; set; }
    public bool? EsSync { get; set; } // Nuevo campo para distinguir sincronización
}