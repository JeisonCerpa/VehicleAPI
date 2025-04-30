const API_URL = "https://vehicleapi-production.up.railway.app/api/vehicle";

function limpiarCampoFecha(valor) {
  if (valor === undefined || valor === null) return "";
  if (typeof valor === "string") return valor.trim() === "" ? "" : valor.trim();
  if (Object.prototype.toString.call(valor) === "[object Date]") {
    return Utilities.formatDate(valor, Session.getScriptTimeZone(), "dd/MM/yyyy");
  }
  if (typeof valor === "number") {
    var fecha = new Date(Math.round((valor - 25569) * 86400 * 1000));
    return Utilities.formatDate(fecha, Session.getScriptTimeZone(), "dd/MM/yyyy");
  }
  return "";
}

function onFormSubmit(e) {
  var hoja = SpreadsheetApp.getActiveSpreadsheet().getActiveSheet();
  var datosFila = e.values;
  var fila = e.range.getRow();

  var datos = {
    MarcaTemporal: datosFila[0],
    Placa: datosFila[1],
    NombreConductor: datosFila[2],
    TarjetaPropiedad: datosFila[3],
    Soat: datosFila[4],
    SeguridadSocial: datosFila[5],
    LicenciaConduccion: datosFila[6],
    CertificadoRevision: datosFila[7],
    FechaVencimientoLicencia: limpiarCampoFecha(datosFila[8]),
    PerteneceEmpresa: datosFila[9],
    FechaVencimientoSoat: limpiarCampoFecha(datosFila[10]),
    FechaVencimientoRevision: limpiarCampoFecha(datosFila[11]),
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
  var status = respuesta.getResponseCode();
  var syncCol = datosFila.length + 1; // Columna Sync (la siguiente a la última columna de datos)

  if (status === 200 || status === 201) {
    hoja.getRange(fila, syncCol).setValue("OK");
  } else {
    hoja.getRange(fila, syncCol).setValue("ERROR");
  }
  Logger.log(respuesta.getContentText());
}

// Sincroniza todas las filas que no tengan "OK" en la columna Sync
function syncPendientes() {
  var hoja = SpreadsheetApp.getActiveSpreadsheet().getActiveSheet();
  var datos = hoja.getDataRange().getValues();
  var syncCol = datos[0].length; // Última columna (Sync)
  for (var i = 1; i < datos.length; i++) { // Empieza en 1 para saltar encabezados
    if (datos[i][syncCol - 1] !== "OK") {
      var fila = i + 1;
      var datosFila = datos[i];
      var registro = {
        MarcaTemporal: datosFila[0],
        Placa: datosFila[1],
        NombreConductor: datosFila[2],
        TarjetaPropiedad: datosFila[3],
        Soat: datosFila[4],
        SeguridadSocial: datosFila[5],
        LicenciaConduccion: datosFila[6],
        CertificadoRevision: datosFila[7],
        FechaVencimientoLicencia: limpiarCampoFecha(datosFila[8]),
        PerteneceEmpresa: datosFila[9],
        FechaVencimientoSoat: limpiarCampoFecha(datosFila[10]),
        FechaVencimientoRevision: limpiarCampoFecha(datosFila[11]),
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
        NombreResponsableVehiculoCC: datosFila[51],
        EsSync: true // Solo en sincronización masiva
      };

      var opciones = {
        method: "post",
        contentType: "application/json",
        payload: JSON.stringify(registro),
        muteHttpExceptions: true
      };

      var respuesta = UrlFetchApp.fetch(API_URL, opciones);
      var status = respuesta.getResponseCode();
      if (status === 200 || status === 201) {
        hoja.getRange(fila, syncCol).setValue("OK");
      } else {
        hoja.getRange(fila, syncCol).setValue("ERROR");
      }
      Logger.log("Fila " + fila + ": " + respuesta.getContentText());
    }
  }
}

function crearTriggerDiario() {
  // Crea un trigger para ejecutar syncPendientes todos los días a las 2:00 AM
  ScriptApp.newTrigger('syncPendientes')
    .timeBased()
    .atHour(2)
    .everyDays(1)
    .create();
}
