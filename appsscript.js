const API_URL = "https://vehicleapi-production.up.railway.app/api/vehicle";

function onFormSubmit(e) {
  var datosFila = e.values; // Último registro enviado por el formulario

  var datos = {
    MarcaTemporal: datosFila[0],
    Placa: datosFila[1],
    NombreConductor: datosFila[2],
    TarjetaPropiedad: datosFila[3],
    Soat: datosFila[4],
    SeguridadSocial: datosFila[5],
    LicenciaConduccion: datosFila[6],
    CertificadoRevision: datosFila[7],
    PerteneceEmpresa: datosFila[8],
    FechaVencimientoSoat: datosFila[9],
    FechaVencimientoRevision: datosFila[10],
    FechaVencimientoLicencia: datosFila[11],
    EntregaRecibe: datosFila[12],
    FotoFrontalVehiculo: datosFila[13],
    FotoLateralDerechoVehiculo: datosFila[14],
    FotoLateralIzquierdoVehiculo: datosFila[15],
    FotoTraseraVehiculo: datosFila[16],
    CarroceriaLatoneriaPuertas: datosFila[17],
    CarroceriaMarcoCarcasaMotos: datosFila[18],
    CarroceriaSuspension: datosFila[19],
    CarroceriaAsientosCojineria: datosFila[20],
    CarroceriaSegurosBloqueosPuertas: datosFila[21],
    CarroceriaLimpiabrisas: datosFila[22],
    CarroceriaVidrios: datosFila[23],
    CarroceriaRetrovisores: datosFila[24],
    ElementosLabradoLlantas: datosFila[25],
    ElementosPresionLlantas: datosFila[26],
    ElementosNivelLiquidos: datosFila[27],
    ElementosAusenciaFugas: datosFila[28],
    ElementosPedales: datosFila[29],
    ElementosFrenosDiscoMotos: datosFila[30],
    ElementosCadenaMotos: datosFila[31],
    ElementosManijasFrenoClutchMotos: datosFila[32],
    ElectricoFarolas: datosFila[33],
    ElectricoDireccionalesParqueo: datosFila[34],
    ElectricoLucesReversa: datosFila[35],
    ElectricoLucesFrenoPare: datosFila[36],
    ElectricoIndicadoresTablero: datosFila[37],
    ElectricoPito: datosFila[38],
    ElectricoAlarmaReversa: datosFila[39],
    PrevencionCinturonesSeguridad: datosFila[40],
    PrevencionAlarmaSeguridad: datosFila[41],
    PrevencionKitCarretera: datosFila[42],
    PrevencionExtintor: datosFila[43],
    PrevencionLlantaRepuesto: datosFila[44],
    PrevencionChalecoReflectivo: datosFila[45],
    PrevencionCasco: datosFila[46],
    VehiculoAutorizadoTransitar: datosFila[47],
    FactoresImpidenMovilizacion: datosFila[48],
    Observaciones: datosFila[49],
    NombreConductorCC: datosFila[50],
    NombreResponsableVehiculoCC: datosFila[51]
  };

  var opciones = {
    method: "post",
    contentType: "application/json",
    payload: JSON.stringify(datos),
    muteHttpExceptions: true
  };

  var respuesta = UrlFetchApp.fetch(API_URL, opciones);
  Logger.log(respuesta.getContentText());
}
