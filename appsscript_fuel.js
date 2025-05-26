const FUEL_API_URL = "https://vehicleapi-production.up.railway.app/api/fuelsupply";

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

function enviarAbastecimientoCombustible(datosFila, esSync) {
  var datos = {
    MarcaTemporal: limpiarCampoFecha(datosFila[0]),
    PlacasDelVehiculo: datosFila[1],
    TipoCombustible: datosFila[2],
    Kilometraje: datosFila[3],
    CantidadGalones: datosFila[4],
    ValorCombustible: datosFila[5],
    DiligenciadoPor: datosFila[6],
    EsSync: esSync === true
  };

  var opciones = {
    method: "post",
    contentType: "application/json",
    payload: JSON.stringify(datos),
    muteHttpExceptions: true
  };

  var respuesta = UrlFetchApp.fetch(FUEL_API_URL, opciones);
  Logger.log(respuesta.getContentText());
  return respuesta.getResponseCode();
}

// Sincroniza todas las filas que no tengan "OK" en la columna Sync para combustible
function syncCombustiblePendientes() {
  var hoja = SpreadsheetApp.getActiveSpreadsheet().getActiveSheet();
  var datos = hoja.getDataRange().getValues();
  var syncCol = datos[0].length; // Última columna (Sync)
  for (var i = 1; i < datos.length; i++) { // Empieza en 1 para saltar encabezados
    if (datos[i][syncCol - 1] !== "OK") {
      var fila = i + 1;
      var datosFila = datos[i];
      var status = enviarAbastecimientoCombustible(datosFila, true);
      if (status === 200 || status === 201) {
        hoja.getRange(fila, syncCol).setValue("OK");
      } else {
        hoja.getRange(fila, syncCol).setValue("ERROR");
      }
      Logger.log("Fila " + fila + ": " + status);
    }
  }
}

// Ejemplo de uso para formulario:
// function onFuelFormSubmit(e) {
//   var datosFila = e.values;
//   var status = enviarAbastecimientoCombustible(datosFila, false);
//   // Puedes marcar la fila como OK/ERROR según status
// }
