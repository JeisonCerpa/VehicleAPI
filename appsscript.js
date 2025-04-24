const API_URL = "https://spatial-spam-mountains-foul.trycloudflare.com/api/vehicle"; // URL pública de tu API

function enviarTodasLasFilas() {
  const hoja = SpreadsheetApp.getActiveSpreadsheet().getActiveSheet();
  const datos = hoja.getDataRange().getValues();
  for (let i = 1; i < datos.length; i++) {
    const fila = datos[i];
    const data = {
      marcaTemporal: new Date(fila[0]).toISOString(),
      placa: fila[2],
      nombreConductor: fila[3]
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
  const fila = e.values;
  let fecha = fila[0];
  let fechaISO = "";
  try {
    // Si ya es formato ISO
    if (fecha.match(/\d{4}-\d{2}-\d{2}T/)) {
      fechaISO = fecha;
    } else {
      // Intenta parsear formato dd/MM/yyyy HH:mm:ss o similar
      let partes = fecha.split(/[\/ :]/);
      if (partes.length >= 5) {
        let d = partes[0], m = partes[1], y = partes[2], h = partes[3], min = partes[4], s = partes[5] || "00";
        fechaISO = new Date(`${y}-${m}-${d}T${h}:${min}:${s}`).toISOString();
      } else {
        fechaISO = new Date(fecha).toISOString();
      }
    }
  } catch (err) {
    fechaISO = new Date().toISOString();
  }

  const data = {
    marcaTemporal: fechaISO,
    placa: fila[2],
    nombreConductor: fila[3]
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
