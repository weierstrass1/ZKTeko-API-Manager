# ZKTeko-API-Manager

Este proyecto tiene como finalidad el envio de marcas desde la Base de datos de ZKTeco a la API de CTRL.

## Settings

Incluye un archivo settings.json similar al siguiente:

```
{
  "DBServerName": "*****",
  "DBServerIP": "*****",
  "DBUsername": "*****",
  "DBPassword": "*****",
  "APIUrlReSend": "????",
  "APIUrlHour": "????",
  "APISuccessMessage": "Exito",
  "DBIgnoreMessages": "('Exito','duplicado','Reenviado')",
  "Days": 0,
  "SerialsNumber": "('CJIK220760004')",
  "MaxDiffTime": 5,
  //Si DateRange es null, usa Days sino usa Range
  "DateRange": null,
  //"DateRange": [ "2022-05-18T00:00:00", "2022-05-19T00:00:00" ],
  "ReSendStatus": "Reenviado",
  "SendLog": false,
  "Ignore": true,
  "OnlySerialsInList": false,
  "OnlyNewRegs": false,
  "OnlyNotNewReg": false
}
```
  
- DBServerName: Nombre de la base de datos.
- DBServerIP: IP de la base de datos.
- DBUsername: Usuario de la base de datos.
- DBPassword: Password de la base de datos.
- APIUrlReSend: Direccion URL de la API de CTRL para recibir registros de la base de datos.
- APIUrlHour: Direccion URL de la API de CTRL para obtener los millisegundos de la aplicacion de CTRL. Solo util con la opcion --hour.
- APISuccessMessage: Mensaje de exito que debiera recibir desde la API de CTRL.
- DBIgnoreMessages: Los registros de la base de datos que tengan como respuesta alguno de los mensajes de esta lista, seran ignorados cuando se utilice el comando --ignore.
- Days: Cantidad de dias para revisar en la base de datos, 0 es el dia actual.
- SerialsNumber: Numeros seriales de dispositivos que se utilizan para filtrar cuando se usa el comando --serial.
- MaxDiffTime: Cantidad de tiempo que se considera valido entre la diferencia de la plataforma y el servidor. Actualmente esta deprecado.
- DateRange: Rango de tiempo que se usa para envio de registros, si es null se utiliza el parametro days, si no es null se utiliza el rango de tiempo.
- ReSendStatus: Respuesta que se pone en la base de datos al reenviar un mensaje.
- SendLog: Si es true, se generara un archivo log con informacion de utilidad.
- Ignore: Si es true, se filtraran los registros por Respuesta ignorando las respuestas en la lista en el setting DBIgnoreMessages.
- OnlySerialsInList: Si es true, se filtraran los registros por numero serial utilizando la lista en el setting SerialsNumber.
- OnlyNewRegs: Si es true, se filtrara la lista para que solo de registros cuya respuesta sea nula.
- OnlyNotNewReg:  Si es true, se filtrara la lista para que solo de registros cuya respuesta no sea nula. Si esta OnlyNewRegs en true, es ignorado este parametro.

## Opciones

- --hour: Al usarlo, te dara la hora de la plataforma de CTRL y de el servidor de la DB en UTC.
