namespace VehicleAPI.Models;

public class RegistroVehiculo
{
    public int Id { get; set; }
    public DateTime MarcaTemporal { get; set; }
    public string? Placa { get; set; }
    public string? NombreConductor { get; set; }
    public string? TarjetaPropiedad { get; set; }
    public string? Soat { get; set; }
    public string? SeguridadSocial { get; set; }
    public string? LicenciaConduccion { get; set; }
    public string? CertificadoRevision { get; set; }
    public DateTime? FechaVencimientoLicencia { get; set; }
    public string? PerteneceEmpresa { get; set; }
    public DateTime? FechaVencimientoSoat { get; set; }
    public DateTime? FechaVencimientoRevision { get; set; }
    public string? EntregaRecibe { get; set; }
    public string? FotoFrontalVehiculo { get; set; }
    public string? FotoLateralDerechoVehiculo { get; set; }
    public string? FotoLateralIzquierdoVehiculo { get; set; }
    public string? FotoTraseraVehiculo { get; set; }
    public string? CarroceriaLatoneriaPuertas { get; set; }
    public string? CarroceriaMarcoCarcasaMotos { get; set; }
    public string? CarroceriaSuspension { get; set; }
    public string? CarroceriaAsientosCojineria { get; set; }
    public string? CarroceriaSegurosBloqueosPuertas { get; set; }
    public string? CarroceriaLimpiabrisas { get; set; }
    public string? CarroceriaVidrios { get; set; }
    public string? CarroceriaRetrovisores { get; set; }
    public string? ElementosLabradoLlantas { get; set; }
    public string? ElementosPresionLlantas { get; set; }
    public string? ElementosNivelLiquidos { get; set; }
    public string? ElementosAusenciaFugas { get; set; }
    public string? ElementosPedales { get; set; }
    public string? ElementosFrenosDiscoMotos { get; set; }
    public string? ElementosCadenaMotos { get; set; }
    public string? ElementosManijasFrenoClutchMotos { get; set; }
    public string? ElectricoFarolas { get; set; }
    public string? ElectricoDireccionalesParqueo { get; set; }
    public string? ElectricoLucesReversa { get; set; }
    public string? ElectricoLucesFrenoPare { get; set; }
    public string? ElectricoIndicadoresTablero { get; set; }
    public string? ElectricoPito { get; set; }
    public string? ElectricoAlarmaReversa { get; set; }
    public string? PrevencionCinturonesSeguridad { get; set; }
    public string? PrevencionAlarmaSeguridad { get; set; }
    public string? PrevencionKitCarretera { get; set; }
    public string? PrevencionExtintor { get; set; }
    public string? PrevencionLlantaRepuesto { get; set; }
    public string? PrevencionChalecoReflectivo { get; set; }
    public string? PrevencionCasco { get; set; }
    public string? VehiculoAutorizadoTransitar { get; set; }
    public string? FactoresImpidenMovilizacion { get; set; }
    public string? Observaciones { get; set; }
    public string? NombreConductorCC { get; set; }
    public string? NombreResponsableVehiculoCC { get; set; }
}

public class RegistroVehiculoDto
{
    public string MarcaTemporal { get; set; } = string.Empty;
    public string? Placa { get; set; }
    public string? NombreConductor { get; set; }
    public string? TarjetaPropiedad { get; set; }
    public string? Soat { get; set; }
    public string? SeguridadSocial { get; set; }
    public string? LicenciaConduccion { get; set; }
    public string? CertificadoRevision { get; set; }
    public string? FechaVencimientoLicencia { get; set; }
    public string? PerteneceEmpresa { get; set; }
    public string? FechaVencimientoSoat { get; set; }
    public string? FechaVencimientoRevision { get; set; }
    public string? EntregaRecibe { get; set; }
    public string? FotoFrontalVehiculo { get; set; }
    public string? FotoLateralDerechoVehiculo { get; set; }
    public string? FotoLateralIzquierdoVehiculo { get; set; }
    public string? FotoTraseraVehiculo { get; set; }
    public string? CarroceriaLatoneriaPuertas { get; set; }
    public string? CarroceriaMarcoCarcasaMotos { get; set; }
    public string? CarroceriaSuspension { get; set; }
    public string? CarroceriaAsientosCojineria { get; set; }
    public string? CarroceriaSegurosBloqueosPuertas { get; set; }
    public string? CarroceriaLimpiabrisas { get; set; }
    public string? CarroceriaVidrios { get; set; }
    public string? CarroceriaRetrovisores { get; set; }
    public string? ElementosLabradoLlantas { get; set; }
    public string? ElementosPresionLlantas { get; set; }
    public string? ElementosNivelLiquidos { get; set; }
    public string? ElementosAusenciaFugas { get; set; }
    public string? ElementosPedales { get; set; }
    public string? ElementosFrenosDiscoMotos { get; set; }
    public string? ElementosCadenaMotos { get; set; }
    public string? ElementosManijasFrenoClutchMotos { get; set; }
    public string? ElectricoFarolas { get; set; }
    public string? ElectricoDireccionalesParqueo { get; set; }
    public string? ElectricoLucesReversa { get; set; }
    public string? ElectricoLucesFrenoPare { get; set; }
    public string? ElectricoIndicadoresTablero { get; set; }
    public string? ElectricoPito { get; set; }
    public string? ElectricoAlarmaReversa { get; set; }
    public string? PrevencionCinturonesSeguridad { get; set; }
    public string? PrevencionAlarmaSeguridad { get; set; }
    public string? PrevencionKitCarretera { get; set; }
    public string? PrevencionExtintor { get; set; }
    public string? PrevencionLlantaRepuesto { get; set; }
    public string? PrevencionChalecoReflectivo { get; set; }
    public string? PrevencionCasco { get; set; }
    public string? VehiculoAutorizadoTransitar { get; set; }
    public string? FactoresImpidenMovilizacion { get; set; }
    public string? Observaciones { get; set; }
    public string? NombreConductorCC { get; set; }
    public string? NombreResponsableVehiculoCC { get; set; }
    public bool? EsSync { get; set; } // Nuevo campo para distinguir sincronización
}
