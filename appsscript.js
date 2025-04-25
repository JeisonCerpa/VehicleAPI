const API_URL = "https://microwave-sing-sciences-starts.trycloudflare.com ";

function getColIndexByName(cabecera, nombreColumna) {
  return cabecera.indexOf(nombreColumna);
}

function enviarTodasLasFilas() {
  const hoja = SpreadsheetApp.getActiveSpreadsheet().getActiveSheet();
  const datos = hoja.getDataRange().getValues();
  const cabecera = datos[0];

  const idxFecha = getColIndexByName(cabecera, "Marca temporal");
  const idxPlaca = getColIndexByName(cabecera, "Placa:");
  const idxConductor = getColIndexByName(cabecera, "Nombre Conductor:");

  for (let i = 1; i < datos.length; i++) {
    const fila = datos[i];
    const data = {
      marcaTemporal: fila[idxFecha], // Enviar la fecha tal como está
      placa: fila[idxPlaca],
      nombreConductor: fila[idxConductor]
    };
    const options = {
      method: "POST",
      contentType: "application/json",
      payload: JSON.stringify(data)
    };
    try {
      const response = UrlFetchApp.fetch(API_URL, options);
      Logger.log("Datos enviados: " + response.getContentText());
    } catch (error) {
      Logger.log("Error al enviar fila: " + error.toString());
    }
  }
  Logger.log("Envío completado.");
}

function onFormSubmit(e) {
  const hoja = SpreadsheetApp.getActiveSpreadsheet().getActiveSheet();
  const cabecera = hoja.getDataRange().getValues()[0];

  const idxFecha = getColIndexByName(cabecera, "Marca temporal");
  const idxPlaca = getColIndexByName(cabecera, "Placa:");
  const idxConductor = getColIndexByName(cabecera, "Nombre Conductor:");

  const fila = e.values;
  const data = {
    marcaTemporal: fila[idxFecha], // Enviar la fecha tal como está
    placa: fila[idxPlaca],
    nombreConductor: fila[idxConductor]
  };

  const options = {
    method: "POST",
    contentType: "application/json",
    payload: JSON.stringify(data)
  };

  try {
    const response = UrlFetchApp.fetch(API_URL, options);
    Logger.log("Datos enviados: " + response.getContentText());
  } catch (error) {
    Logger.log("Error al enviar fila: " + error.toString());
  }
}
